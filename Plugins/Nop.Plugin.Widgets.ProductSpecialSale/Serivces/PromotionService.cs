using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public class PromotionService : IPromotionService
    {

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PromotionProduct> _promotionProductRepository;
        private readonly IRepository<Promotion> _promotionRepository;

        public PromotionService(IRepository<Promotion> promotionRepository,
            IRepository<PromotionProduct> promotionProductRepository, IEventPublisher eventPublisher)
        {
            _promotionRepository = promotionRepository;
            _promotionProductRepository = promotionProductRepository;
            _eventPublisher = eventPublisher;
        }

        public IList<Promotion> GetAllPromotions()
        {
            return _promotionRepository.Table.Where(a => !a.Deleted).ToList();
        }

        public virtual IPagedList<Promotion> SearchPromotions(string billNumber, string billName, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc, PromotionType? promotionType, PromotionStatus? promotionStatus, bool? isEnabled, int pageIndex, int pageSize)
        {
            int? billTypeId = null;
            if (promotionType.HasValue)
                if ((int)promotionType.Value != 0)
                    billTypeId = (int)promotionType.Value;
            int? billStatusId = null;
            if (promotionStatus.HasValue)
                billStatusId = (int)promotionStatus.Value;
            IQueryable<Promotion> query = _promotionRepository.Table.Where(a => !a.Deleted);

            if (startDateTimeUtc.HasValue)
                query = query.Where(o => startDateTimeUtc.Value <= o.StartDateTimeUtc);
            if (endDateTimeUtc.HasValue)
                query = query.Where(o => endDateTimeUtc.Value >= o.EndDateTimeUtc);
            if (billTypeId.HasValue)
                query = query.Where(o => promotionType == o.Type);
            if (billStatusId.HasValue)
                query = query.Where(o => promotionStatus == o.Status);
            if (!String.IsNullOrEmpty(billName))
                query = query.Where(o => o.Name.Contains(billName));
            if (isEnabled != null)
            {
                query = query.Where(o => o.IsEnabled == isEnabled);
            }
            query = query.OrderByDescending(o => o.CreatedOnUtc);
            return new PagedList<Promotion>(query, pageIndex, pageSize);
        }

        public virtual Promotion GetPromotionById(int promotionId)
        {
            return promotionId == 0 ? null : _promotionRepository.GetById(promotionId);
        }

        /// <summary>
        /// 根据商品ID搜索该商品正处在那个促销单中
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual Promotion GetPromotionByProductId(int productId)
        {
            var promotionIds =
                _promotionProductRepository.Table.Where(
                    a =>
                        !a.Promotion.Deleted && a.Promotion.IsEnabled &&
                        (a.Promotion.Status == PromotionStatus.New || a.Promotion.Status == PromotionStatus.OnPromotion) && a.ProductId == productId).Select(a => a.PromotionId).Distinct().ToList();
            return _promotionRepository.Table.FirstOrDefault(a => !a.Deleted && a.IsEnabled &&
                                                                  (a.StartDateTimeUtc <= DateTime.UtcNow && a.EndDateTimeUtc > DateTime.UtcNow) && promotionIds.Contains(a.Id));
        }

        public virtual void InsertPromotion(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException("promotion");
            promotion.CreatedOnUtc = DateTime.UtcNow;
            promotion.LastUpdatedDateTimeUtc = DateTime.UtcNow;
            _promotionRepository.Insert(promotion);
            //event notification
            _eventPublisher.EntityInserted(promotion);
        }

        public virtual void UpdatePromotion(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException("promotion");
            if (promotion.Status != PromotionStatus.New) return;
            promotion.LastUpdatedDateTimeUtc = DateTime.UtcNow;
            _promotionRepository.Update(promotion);
            //event notification
            _eventPublisher.EntityUpdated(promotion);
        }

        public virtual void DeletePromotion(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException("promotion");
            if (promotion.Status == PromotionStatus.OnPromotion) return;
            promotion.Deleted = true;
            _promotionRepository.Update(promotion);
            //event notification
            _eventPublisher.EntityUpdated(promotion);
        }

        public virtual IPagedList<PromotionProduct> GetPromotionProductsByPromotionId(int promotionId,
            string searchCondition, int pageIndex, int pageSize)
        {
            IQueryable<PromotionProduct> query = from rp in _promotionProductRepository.Table
                                                 join p in _promotionRepository.Table on rp.PromotionId equals p.Id
                                                 where rp.PromotionId == promotionId &&
                                                 !p.Deleted
                                                 select rp;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                query = query.Where(a => a.Product.Name.Contains(searchCondition));
            }
            query = query.OrderByDescending(p => p.Product.CreatedOnUtc);
            if (pageIndex == 0 && pageSize == 0)
            {
                return new PagedList<PromotionProduct>(query, pageIndex, 1000000);
            }
            return new PagedList<PromotionProduct>(query, pageIndex, pageSize);
        }

        public virtual IList<PromotionProduct> GetPromotionProductsByPromotionDate(DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            var query = from rp in _promotionProductRepository.Table
                        join p in _promotionRepository.Table on rp.PromotionId equals p.Id
                        where !p.Deleted
                        select rp;
            return query.Where(
                    a =>
                        (a.Promotion.StartDateTimeUtc == startDateTimeUtc) ||
                        (a.Promotion.StartDateTimeUtc > startDateTimeUtc &&
                         a.Promotion.StartDateTimeUtc < endDateTimeUtc) ||
                        (a.Promotion.StartDateTimeUtc < startDateTimeUtc &&
                         a.Promotion.EndDateTimeUtc >= startDateTimeUtc)).ToList();
        }

        public virtual PromotionProduct GetPromotionProductById(int promotionProductId)
        {
            return promotionProductId == 0 ? null : _promotionProductRepository.GetById(promotionProductId);
        }

        public virtual void InsertPromotionProduct(PromotionProduct promotionProduct)
        {
            if (promotionProduct == null)
                throw new ArgumentNullException("promotionProduct");
            Promotion promotion = _promotionRepository.Table.FirstOrDefault(a => a.Id == promotionProduct.PromotionId);
            if (promotion == null || promotion.Status != PromotionStatus.New) return;
            _promotionProductRepository.Insert(promotionProduct);

            //event notification
            _eventPublisher.EntityInserted(promotionProduct);
        }

        public virtual void UpdatePromotionProduct(PromotionProduct promotionProduct)
        {
            if (promotionProduct == null)
                throw new ArgumentNullException("promotionProduct");
            Promotion promotion = promotionProduct.Promotion;
            if (promotion == null || promotion.Status != PromotionStatus.New) return;
            _promotionProductRepository.Update(promotionProduct);
            //event notification
            _eventPublisher.EntityUpdated(promotionProduct);
        }

        public virtual void DeletePromotionProduct(PromotionProduct promotionProduct)
        {
            if (promotionProduct == null)
                throw new ArgumentNullException("promotionProduct");
            Promotion promotion = promotionProduct.Promotion;
            if (promotion == null || promotion.Status == PromotionStatus.OnPromotion) return;
            _promotionProductRepository.Delete(promotionProduct);
            //event notification
            _eventPublisher.EntityDeleted(promotionProduct);
        }

        public IList<Promotion> GetProcessingPromotions()
        {
            return _promotionRepository.Table
                .Where(p => (p.Status == PromotionStatus.New || p.Status == PromotionStatus.OnPromotion)
                            && p.IsEnabled && !p.Deleted).ToList();
        }

        public void StartPromotion(Promotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException("promotion");
            }
            if (promotion.Status != PromotionStatus.New)
            {
                throw new Exception("Promotion must be New status.");
            }
            promotion.Status = PromotionStatus.OnPromotion;
            AddPromotionStartedTracking(promotion);
            foreach (PromotionProduct promotionProduct in promotion.Products)
            {
                AddPromotionProductTracking(promotion, promotionProduct);
                StartProductPromotion(promotion, promotionProduct);
            }
            _promotionRepository.Update(promotion);
        }

        public void CompletePromotion(Promotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException("promotion");
            }
            if (promotion.Status != PromotionStatus.OnPromotion)
            {
                throw new Exception("Promotion must be OnPromotion status.");
            }
            promotion.Status = PromotionStatus.Completed;
            AddPromotionCompletedTracking(promotion);
            foreach (PromotionProductTracking productTracking in promotion.ProductTrackings)
            {
                CompleteProductPromotion(productTracking);
            }
            _promotionRepository.Update(promotion);
        }

        private static void AddPromotionStartedTracking(Promotion promotion)
        {
            var promotionExecutionTracking = new PromotionTracking
            {
                CreatedOnUtc = DateTime.UtcNow,
                Type = PromotionTrackingType.Started,
                Remark = "促销开始啦！"
            };
            promotion.Trackings.Add(promotionExecutionTracking);
        }

        private static void AddPromotionCompletedTracking(Promotion promotion)
        {
            var promotionExecutionTracking = new PromotionTracking
            {
                CreatedOnUtc = DateTime.UtcNow,
                Type = PromotionTrackingType.Completed,
                Remark = "促销已完成。"
            };
            promotion.Trackings.Add(promotionExecutionTracking);
        }

        private static void AddPromotionProductTracking(Promotion promotion, PromotionProduct promotionProduct)
        {
            var promotionProductTracking = new PromotionProductTracking
            {
                SpecialPriceStartDateTimeUtc = promotion.StartDateTimeUtc,
                SpecialPriceEndDateTimeUtc = promotion.EndDateTimeUtc,
                ProductId = promotionProduct.ProductId,
                SpecialPrice = promotionProduct.PcPlatformPrice,
                ManageInventoryMethodId = (int)ManageInventoryMethod.ManageStock,
                MinStockQuantity = promotionProduct.Product.StockQuantity - promotionProduct.Quantity,
                LowStockActivityId = (int)LowStockActivity.Nothing,
                CustomerMaximumQuantity = promotionProduct.PcPlatformCustomerMaximumQuantity,
                OriginalSpecialPrice = promotionProduct.Product.SpecialPrice,
                OriginalSpecialPriceStartDateTimeUtc = promotionProduct.Product.SpecialPriceStartDateTimeUtc,
                OriginalSpecialPriceEndDateTimeUtc = promotionProduct.Product.SpecialPriceEndDateTimeUtc,
                OriginalManageInventoryMethodId = promotionProduct.Product.ManageInventoryMethodId,
                OriginalMinStockQuantity = promotionProduct.Product.MinStockQuantity,
                OriginalLowStockActivityId = promotionProduct.Product.LowStockActivityId,
                //TODO:字段添加
                //OriginalCustomerMaximumQuantity = promotionProduct.Product.CustomerMaximumQuantity,
                
                OriginalPublished = promotionProduct.Product.Published,
                OriginalDisableBuyButton = promotionProduct.Product.DisableBuyButton,
                OriginalDisableWishlistButton = promotionProduct.Product.DisableWishlistButton
            };
            promotion.ProductTrackings.Add(promotionProductTracking);
        }

        private static void StartProductPromotion(Promotion promotion, PromotionProduct promotionProduct)
        {
            promotionProduct.Product.SpecialPriceStartDateTimeUtc = promotion.StartDateTimeUtc;
            promotionProduct.Product.SpecialPriceEndDateTimeUtc = promotion.EndDateTimeUtc;
            promotionProduct.Product.SpecialPrice = promotionProduct.PcPlatformPrice;
            
            promotionProduct.Product.ManageInventoryMethodId = (int)ManageInventoryMethod.ManageStock;
            promotionProduct.Product.MinStockQuantity = promotionProduct.Product.StockQuantity -
                                                        promotionProduct.Quantity;
            promotionProduct.Product.LowStockActivityId = (int)LowStockActivity.Nothing;
            //TODO:字段添加
            //promotionProduct.Product.CustomerMaximumQuantity = promotionProduct.PcPlatformCustomerMaximumQuantity;
            //promotionProduct.Product.IsOnPromotion = true;
            //promotionProduct.Product.SpecialStockTotalQuantity = promotionProduct.Quantity;
        }

        private static void CompleteProductPromotion(PromotionProductTracking productTracking)
        {
            productTracking.Product.SpecialPriceStartDateTimeUtc = productTracking.OriginalSpecialPriceStartDateTimeUtc;
            productTracking.Product.SpecialPriceEndDateTimeUtc = productTracking.OriginalSpecialPriceEndDateTimeUtc;
            productTracking.Product.SpecialPrice = productTracking.OriginalSpecialPrice;
            productTracking.Product.ManageInventoryMethodId = productTracking.OriginalManageInventoryMethodId;
            productTracking.Product.MinStockQuantity = productTracking.OriginalMinStockQuantity;
            productTracking.Product.LowStockActivityId = productTracking.OriginalLowStockActivityId;
            //TODO:字段添加
            //productTracking.Product.CustomerMaximumQuantity = productTracking.OriginalCustomerMaximumQuantity;
            //productTracking.Product.IsOnPromotion = false;
            productTracking.Product.Published = productTracking.OriginalPublished;
            productTracking.Product.DisableBuyButton = productTracking.OriginalDisableBuyButton;
            productTracking.Product.DisableWishlistButton = productTracking.OriginalDisableWishlistButton;
           
        }
    }
}
