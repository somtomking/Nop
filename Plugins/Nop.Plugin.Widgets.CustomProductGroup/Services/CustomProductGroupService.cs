using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;

using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using G = Nop.Plugin.Widgets.CustomProductGroup.Domain;
using Nop.Plugin.Widgets.CustomProductGroup.Domain;
 

namespace Nop.Plugin.Widgets.CustomProductGroup.Services
{
    public partial class CustomProductGroupService : ICustomProductGroupService
    {
        #region Constants

        private const string CUSTOMPRODUCTGROUP_ALL_KEY = "Nop.customproductgroup.all-{0}-{1}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : CustomProductGroup ID
        /// </remarks>
        private const string CUSTOMPRODUCTGROUPS_BY_ID_KEY = "Nop.customproductgroup.id-{0}";

        private const string CUSTOMPRODUCTGROUPITEMS_BY_ID_KEY = "Nop.customproductgroupitem.all-{0}";

        /// <summary>
        /// 首页商品分类列表
        /// </summary>
        private const string CUSTOMPRODUCTGROUP_PUBLICINFO_KEY = "Nop.widgets.customproductgroup.publicinfo";
        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<G .CustomProductGroup> _customProductGroupRepository;
        private readonly IRepository<CustomerCustomProductGroup> _customerCustomProductGroupRepository;
        private readonly IRepository<G .CustomProductGroupItem> _customProductGroupItemRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Product> _productRepository;
        #endregion

        #region Ctor

        public CustomProductGroupService(ICacheManager cacheManager, IRepository<G .CustomProductGroup> customProductGroupRepository,
            IRepository<G .CustomProductGroupItem> customProductGroupItemRepository
            , 
            IRepository<CustomerCustomProductGroup> customerCustomProductGroupRepository,
            IRepository<OrderItem> orderItemRepository, IRepository<Product> productRepository)
        {
            this._cacheManager = cacheManager;
            this._customProductGroupRepository = customProductGroupRepository;
            this._customProductGroupItemRepository = customProductGroupItemRepository;
           // this._customerCustomProductGroupRepository = customerCustomProductGroupRepository;
            this._orderItemRepository = orderItemRepository;
            this._productRepository = productRepository;
        }

        #endregion

        #region CustomProductGroup Methods

        public virtual void InsertCustomProductGroup(G .CustomProductGroup customProductGroup)
        {
            if (customProductGroup == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupRepository.Insert(customProductGroup);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityInserted(blogPost);
        }

        public virtual void UpdateCustomProductGroup(G.CustomProductGroup customProductGroup)
        {
            if (customProductGroup == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupRepository.Update(customProductGroup);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityUpdated(customProductGroup);
        }

        public virtual void DeleteCustomProductGroup(G.CustomProductGroup customProductGroup)
        {
            if (customProductGroup == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupRepository.Delete(customProductGroup);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityDeleted(blogPost);
        }

        public virtual G.CustomProductGroup GetCustomProductGroupById(int customProductGroupId)
        {
            if (customProductGroupId == 0)
                return null;

            string key = string.Format(CUSTOMPRODUCTGROUPS_BY_ID_KEY, customProductGroupId);
            return _cacheManager.Get(key, () => { return _customProductGroupRepository.GetById(customProductGroupId); });
        }

        public virtual IList<G.CustomProductGroup> GetAllCustomProductGroups(
            Customer customer = null,
            PlantformType plantformType = PlantformType.Default,
            bool onlyShowEnable = false)
        {
            var query = _customProductGroupRepository.Table;

            if (plantformType != PlantformType.Default)
                query = query.Where(cpg => cpg.Plantform == (int)plantformType || cpg.Plantform == (int)PlantformType.Default);

            if (onlyShowEnable)
                query = query.Where(cpg => cpg.IsEnable);

            var customProductGroups = query.OrderBy(cpg => cpg.DisplayOrder).ToList();
            var sortedCustomProductGroups = new List<G.CustomProductGroup>();//根据 CustomProductGroupIds 排序 CustomProductGroup 列表
            if (customer != null)
            {
                var customerCustomProductGroup = _customerCustomProductGroupRepository.Table.FirstOrDefault(c => c.CustomerId == customer.Id);

                if (customerCustomProductGroup != null
                    && !string.IsNullOrEmpty(customerCustomProductGroup.CustomProductGroupIds))
                {

                    var customProductGroupStringIds = customerCustomProductGroup.CustomProductGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var customProductGroupIds = new List<int>();
                    foreach (var customProductGroupStringId in customProductGroupStringIds)
                    {
                        int customProductGroupId = 0;
                        if (int.TryParse(customProductGroupStringId, out customProductGroupId))//过滤掉非数字字符
                            customProductGroupIds.Add(customProductGroupId);
                    }


                    foreach (var customProductGroupId in customProductGroupIds)
                    {
                        var customProductGroup = customProductGroups.FirstOrDefault(n => n.Id == customProductGroupId);
                        if (customProductGroup != null)
                        {
                            customProductGroup.IsSelected = true;
                            sortedCustomProductGroups.Add(customProductGroup);
                            customProductGroups.Remove(customProductGroup);
                        }
                    }

                    if (customProductGroups.Count > 0)
                        sortedCustomProductGroups.AddRange(customProductGroups);
                }
                else
                {
                    if (customProductGroups.Count > 0)
                        sortedCustomProductGroups.AddRange(customProductGroups);
                }
            }
            else
            {
                if (customProductGroups.Count > 0)
                    sortedCustomProductGroups.AddRange(customProductGroups);
            }

            return sortedCustomProductGroups;
        }

        public virtual void SetCustomerCustomProductGroup(Customer customer, string customProductGroupIds)
        {
            if (customer == null)
                return;

            if (customProductGroupIds != null)
                customProductGroupIds = customProductGroupIds.Trim();

            var customerCustomProductGroup = this._customerCustomProductGroupRepository.Table.FirstOrDefault(cc => cc.CustomerId == customer.Id);
            if (customerCustomProductGroup == null)
            {
                customerCustomProductGroup = new CustomerCustomProductGroup();
                customerCustomProductGroup.CustomerId = customer.Id;
                customerCustomProductGroup.CustomProductGroupIds = customProductGroupIds;
                this._customerCustomProductGroupRepository.Insert(customerCustomProductGroup);
            }
            else
            {
                customerCustomProductGroup.CustomProductGroupIds = customProductGroupIds;
                this._customerCustomProductGroupRepository.Update(customerCustomProductGroup);
            }
        }

        #endregion

        #region CustomProductGroupItem Methods

        public virtual void InsertCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem)
        {
            if (customProductGroupItem == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupItemRepository.Insert(customProductGroupItem);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityInserted(customProductGroupItem);
        }

        public virtual void UpdateCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem)
        {
            if (customProductGroupItem == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupItemRepository.Update(customProductGroupItem);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityUpdated(customProductGroupItem);
        }

        public virtual void DeleteCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem)
        {
            if (customProductGroupItem == null)
                throw new ArgumentNullException("customProductGroup");

            _customProductGroupItemRepository.Delete(customProductGroupItem);

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);

            //event notification
            //_eventPublisher.EntityDeleted(customProductGroupItem);
        }

        public virtual void DeleteCustomProductGroupItems(int customProductGroupId)
        {
            var query = _customProductGroupItemRepository.Table.Where(n => n.CustomProductGroupId == customProductGroupId).ToList();
            query.ForEach(n => _customProductGroupItemRepository.Delete(n));

            _cacheManager.Remove(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY);
        }

        public virtual G.CustomProductGroupItem GetCustomProductGroupItemById(int customProductGroupItemId)
        {
            if (customProductGroupItemId == 0)
                return null;

            string key = string.Format(CUSTOMPRODUCTGROUPITEMS_BY_ID_KEY, customProductGroupItemId);
            return _cacheManager.Get(key, () => { return _customProductGroupItemRepository.GetById(customProductGroupItemId); });
        }

        /// <summary>
        /// 获取所有商品主题项
        /// </summary>
        /// <returns>CustomProductGroupItems</returns>
        public virtual IList<G.CustomProductGroupItem> GetCustomProductGroupItems(
            int customProductGroupId,
            int count,
            PlantformType plantform = PlantformType.Default)
        {
            var queryBase = (from m in _customProductGroupItemRepository.Table where m.CustomProductGroupId == customProductGroupId select m).ToList();
            var query = from n in queryBase
                        join m in _productRepository.Table on n.ProductId equals m.Id
                        where n.CustomProductGroupId == customProductGroupId
                        && m.Published
                        select n;
            

            if (plantform != PlantformType.Default)
                query = query.Where(n => n.Plantform == (int)plantform || n.Plantform == (int)PlantformType.Default);

            return query.OrderBy(n => n.DisplayOrder).Take(count).ToList();
        }

        public virtual IPagedList<Product> GetCustomProductGroupItems(
            int customProductGroupId,
            int pageIndex = 0,
            int pageSize = 2147483647,  //Int32.MaxValue
            ProductSortingEnum orderBy = ProductSortingEnum.Position,
            PlantformType plantform = PlantformType.Default)
        {
            var queryCustomProductGroupItems = from n in _customProductGroupItemRepository.Table
                                               where n.CustomProductGroupId == customProductGroupId
                                               select n;
            //60go
            //if (plantform != PlantformType.Default)
            //    queryCustomProductGroupItems = queryCustomProductGroupItems.Where(n => (n.Plantform == (int)plantform || n.Plantform == (int)PlantformType.Default)
            //        && n.Product.Published && !n.Product.Deleted && ((!n.Product.AvailableEndDateTimeUtc.HasValue) 
            //        || (n.Product.AvailableEndDateTimeUtc.HasValue && n.Product.AvailableEndDateTimeUtc.Value > DateTime.UtcNow)));
             //var queryProducts = queryCustomProductGroupItems.Select(n=>n.Product);

            //self
            if (plantform != PlantformType.Default)
                queryCustomProductGroupItems = queryCustomProductGroupItems.Where(n => (n.Plantform == (int)plantform || n.Plantform == (int)PlantformType.Default));

            var queryProducts = _productRepository.Table.Where(p=> queryCustomProductGroupItems.Select(n => n.ProductId).Contains(p.Id)
                && p.Published && !p.Deleted && ((!p.AvailableEndDateTimeUtc.HasValue) 
                    || (p.AvailableEndDateTimeUtc.HasValue && p.AvailableEndDateTimeUtc.Value > DateTime.UtcNow))
                );

            //if (orderBy == ProductSortingEnum.Position)
            //    query = query.OrderBy(n => n.DisplayOrder);
            if (orderBy == ProductSortingEnum.NameAsc)
                queryProducts = queryProducts.OrderBy(n => n.Name);
            else if (orderBy == ProductSortingEnum.NameDesc)
                queryProducts = queryProducts.OrderByDescending(n => n.Name);
            else if (orderBy == ProductSortingEnum.PriceAsc)
                queryProducts = queryProducts.OrderBy(n => n.Price);
            else if (orderBy == ProductSortingEnum.PriceDesc)
                queryProducts = queryProducts.OrderByDescending(n => n.Price);
            else if (orderBy == ProductSortingEnum.CreatedOn)
                queryProducts = queryProducts.OrderByDescending(n => n.CreatedOnUtc);
            //else if (orderBy == ProductSortingEnum.CommentAsc)
            //{
            //    //ProductRate ASC
            //    queryProducts = queryProducts.OrderBy(p => p.ApprovedRatingSum);
            //}
            //else if (orderBy == ProductSortingEnum.CommentDesc)
            //{
            //    //ProductRate Desc
            //    queryProducts = queryProducts.OrderByDescending(p => p.ApprovedRatingSum);
            //}
            //else if (orderBy == ProductSortingEnum.SellingAsc || orderBy == ProductSortingEnum.SellingDesc)
            //{
            //    var oitemtable = _orderItemRepository.Table;
            //    var orderinfo = oitemtable.GroupBy(p => p.ProductId).Select(x => new { Id = x.Key, SellingCount = x.Sum(t => t.Quantity) });
            //    if (orderBy == ProductSortingEnum.SellingDesc)
            //    {
            //        // orderinfo = orderinfo.OrderByDescending(p => p.SellingCount);
            //        queryProducts = from a in queryProducts
            //                        join b in orderinfo on a.Id equals b.Id into ab
            //                        from b in ab.DefaultIfEmpty()
            //                        orderby b.SellingCount descending
            //                        select a;


            //    }
            //    else
            //    {
            //        queryProducts = from a in queryProducts
            //                        join b in orderinfo on a.Id equals b.Id into ab
            //                        from b in ab.DefaultIfEmpty()
            //                        orderby b.SellingCount ascending
            //                        select a;
            //    }

            //}
            else
            {
                //queryProducts = queryCustomProductGroupItems.OrderBy(n => n.DisplayOrder).Select(n => n.Product);
                //60go
                //queryProducts = queryCustomProductGroupItems.OrderByDescending(n => n.Product.UpdatedOnUtc).Select(n => n.Product);
                //self
                queryProducts = _productRepository.Table.Where(p => queryCustomProductGroupItems.Select(g=>g.ProductId).Contains(p.Id)).OrderByDescending(p => p.UpdatedOnUtc);
            }

            var products = queryProducts.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var totalRecords = queryProducts.Count();

            return new PagedList<Product>(products, pageIndex, pageSize, totalRecords);
        }

        #endregion
    }
}
