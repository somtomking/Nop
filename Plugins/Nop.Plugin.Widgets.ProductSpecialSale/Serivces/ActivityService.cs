using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Core.Data;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public class ActivityService : IActivityService
    {
        #region Fields
        private readonly IRepository<Activity> _activityRepository;
        private readonly IRepository<ActivityProduct> _activityProductRepository;
        private readonly IRepository<ActivityProductGroup> _activityProductGroupRepository;
        private readonly IRepository<ActivityStyle> _activityStyleRepository;
        private readonly IRepository<ActivityComment> _activityCommentRepository;
        private readonly IRepository<ActivityCommentPraise> _activityCommentPraiseRepository;
        private readonly IEventPublisher _eventPublisher;
        #endregion


        #region Ctor
        public ActivityService(IRepository<Activity> activityRepository,
            IRepository<ActivityProduct> activityProductRepository,
            IRepository<ActivityProductGroup> activityProductGroupRepository,
            IRepository<ActivityStyle> activityStyleRepository,
            IRepository<ActivityComment> activityCommentRepository,
            IRepository<ActivityCommentPraise> activityCommentPraiseRepository,
            IEventPublisher eventPublisher)
        {
            this._activityRepository = activityRepository;
            this._activityProductRepository = activityProductRepository;
            this._activityProductGroupRepository = activityProductGroupRepository;
            this._activityStyleRepository = activityStyleRepository;
            this._activityCommentRepository = activityCommentRepository;
            this._activityCommentPraiseRepository = activityCommentPraiseRepository;
            this._eventPublisher = eventPublisher;
        }
        #endregion


        #region IActivityService Members

        public virtual Activity GetActivityByType(int activityType)
        {
            var query = _activityRepository.Table.Where(n => n.Type == activityType);
            return query.FirstOrDefault();
        }

        public virtual Activity GetActivityById(int activityId)
        {
            var query = _activityRepository.Table.Where(n => n.Id == activityId);
            return query.FirstOrDefault();
        }

        public IList<Nop.Plugin.Widgets.ProductSpecialSale.Domain.Activity> GetActivityByAppPlatform()
        {
            var query = _activityRepository.Table.Where(n => n.IsAppPlatformAvailable == true &&
                n.Deleted == false &&
                n.EndTimeUtc >= DateTime.UtcNow);
            //var query = _activityRepository.Table.Where(n => n.IsAppPlatformAvailable == true && n.Deleted == false);
            return query.ToList();
        }

        public IPagedList<Core.Domain.Catalog.Product> GetActivityProducts(int activityid, int pageSize, int pageIndex)
        {
            var table = _activityProductRepository.Table;
            var query = table.Where(
                p =>
                p.ActivityId == activityid && p.IsAppPlatformAvailable == true &&
                (p.Product.AvailableEndDateTimeUtc > DateTime.UtcNow || !p.Product.AvailableEndDateTimeUtc.HasValue) &&
                p.Product.Deleted == false && p.Product.Published)
                             .OrderBy(p => p.Group == null ? 0 : p.Group.DisplayOrder).ThenBy(a => a.DisplayOrder)
                             .ThenBy(p => p.ShowOnHomePage)
                             .Select(p => p.Product);
            return new PagedList<Core.Domain.Catalog.Product>(query, pageIndex, pageSize);
        }

        public IPagedList<Product> GetTopActivityProducts(DateTime? datetime, int pageSize, int pageIndex)
        {
            var table = _activityProductRepository.Table;

            var query = table.Where(p => p.IsPcPlatformAvailable);
            if (datetime != null)
            {
                var maxDateTime = datetime.Value.AddDays(1);
                query = query.Where(m => m.Promotion.StartDateTimeUtc >= datetime.Value && m.Promotion.EndDateTimeUtc < maxDateTime);
            }
            var products = query.OrderBy(p => p.Group == null ? 0 : p.Group.DisplayOrder)
                .ThenBy(p => p.ShowOnHomePage)
                .Select(p => p.Product);
            return new PagedList<Product>(products, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取限时抢购相关的产品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="isPc"></param>
        /// <returns></returns>
        public IList<ActivityProduct> GetActivityProductsByProductId(int productId, bool isPc = true)
        {
            var table = _activityProductRepository.Table;
            var query =
                table.Where(
                    n =>
                    n.Activty.Type == (int)ActivityType.BuyLimit && n.ProductId == productId &&
                    n.IsPcPlatformAvailable == isPc && n.Product.Published
                    && (!n.Product.Deleted)).ToList();
            return query;
        }

        public IList<ActivityProduct> GetActivityProductsByProductId(int productId, int activityId, bool isPc = true)
        {
            var table = _activityProductRepository.Table;
            var query =
                table.Where(
                    n =>
                    n.Activty.Type == (int)ActivityType.BuyLimit && n.ProductId == productId &&
                    n.IsPcPlatformAvailable == isPc && n.ActivityId == activityId && n.Product.Published
                    && (!n.Product.Deleted)).ToList();
            return query;
        }

        #endregion

        #region 根据活动类型,获取对应的活动列表.[author：Zaylor，date:2014-3-10]

        public IList<ActivityProduct> GetProductByActivityType(ActivityType activityType, bool isWechat)
        {
            var table = _activityProductRepository.Table;
            var query = table.Where(m => m.Activty.Type == (int)activityType);
            if (isWechat)
            {
                query = query.Where(m => m.Activty.IsWeixinPlatformAvailable == true);
            }
            return query.ToList();
        }

        #endregion

        #region 获取活动列表.[author：Zaylor，date:2014-3-11]

        /// <summary>
        /// 获取活动列表.[author：Zaylor，date:2014-3-11]
        /// </summary>
        /// <param name="num">前面几条活动信息</param>
        /// <returns>活动列表</returns>
        public IList<Activity> GetActivityTop(int num)
        {
            var table = _activityRepository.Table;
            var query = table.Where(m => m.IsWeixinPlatformAvailable == true);
            return query.Take(num).OrderBy(m => m.Id).ToList();
        }

        #endregion

        public virtual IPagedList<Activity> SearchActivities(string activityName, int? activityCategory, int? activityType, int? activityStatus, bool? isPcPlatformAvailable, bool? isAppPlatformAvailable, bool? isWeixinPlatformAvailable, int pageIndex, int pageSize)
        {
            var query = _activityRepository.Table.Where(a => !a.Deleted);
            //if (startDateTimeUtc.HasValue)
            //    query = query.Where(o => startDateTimeUtc.Value <= o.StartDateTimeUtc);
            //if (endDateTimeUtc.HasValue)
            //    query = query.Where(o => endDateTimeUtc.Value >= o.EndDateTimeUtc);
            if (!String.IsNullOrEmpty(activityName))
                query = query.Where(a => a.Name.Contains(activityName));
            if (activityCategory.HasValue)
                query = query.Where(a => a.Category == activityCategory);
            if (activityType.HasValue)
                query = query.Where(a => a.Type == activityType);
            if (activityStatus.HasValue)
                query = query.Where(a => a.Status == activityStatus);
            if (isPcPlatformAvailable.HasValue)
                query = query.Where(a => a.IsPcPlatformAvailable == isPcPlatformAvailable);
            if (isAppPlatformAvailable.HasValue)
                query = query.Where(a => a.IsAppPlatformAvailable == isAppPlatformAvailable);
            if (isWeixinPlatformAvailable.HasValue)
                query = query.Where(a => a.IsWeixinPlatformAvailable == isWeixinPlatformAvailable);
            query = query.OrderByDescending(a => a.CreatedOnUtc);
            return new PagedList<Activity>(query, pageIndex, pageSize);
        }

        public virtual IPagedList<ActivityProduct> GetActivityProductsByActivityId(int activityId, int? activityProductGroupId,
            string searchCondition, int pageIndex, int pageSize)
        {
            IQueryable<ActivityProduct> query = from ap in _activityProductRepository.Table
                                                join p in _activityRepository.Table on ap.ActivityId equals p.Id
                                                where ap.ActivityId == activityId
                                                select ap;
            if (activityProductGroupId == -1)
            {
                activityProductGroupId = null;
            }
            if (activityProductGroupId != null)
            {
                query = query.Where(a => a.ActivityProductGroupId == activityProductGroupId);
            }
            else
            {
                query = query.Where(a => !a.ActivityProductGroupId.HasValue);
            }
            if (!string.IsNullOrEmpty(searchCondition))
            {
                query = query.Where(a => a.Product.Name.Contains(searchCondition));
            }
            query = query.OrderByDescending(p => p.Id);
            if (pageIndex == 0 && pageSize == 0)
            {
                return new PagedList<ActivityProduct>(query, pageIndex, 1000000);
            }
            return new PagedList<ActivityProduct>(query, pageIndex, pageSize);
        }

        public virtual void InsertActivityProduct(ActivityProduct activityProduct)
        {
            if (activityProduct == null)
                throw new ArgumentNullException("activityProduct");
            var existActivityProduct = _activityProductRepository.Table.FirstOrDefault(a => a.PromotionId == activityProduct.PromotionId && a.ActivityId == activityProduct.ActivityId && a.ProductId == activityProduct.ProductId);
            if (existActivityProduct != null) return;
            _activityProductRepository.Insert(activityProduct);

            //event notification
            _eventPublisher.EntityInserted(activityProduct);
        }

        public virtual void InsertActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");
            activity.Deleted = false;
            activity.CreatedOnUtc = DateTime.UtcNow;
            activity.LastUpdatedTimeUtc = DateTime.UtcNow;
            _activityRepository.Insert(activity);
            //event notification
            _eventPublisher.EntityInserted(activity);
        }

        public virtual void UpdateActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");
            activity.LastUpdatedTimeUtc = DateTime.UtcNow;
            _activityRepository.Update(activity);
            //event notification
            _eventPublisher.EntityUpdated(activity);
        }

        public virtual void DeleteActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");
            activity.Deleted = true;
            _activityRepository.Update(activity);

            //event notification
            _eventPublisher.EntityUpdated(activity);
        }

        public virtual ActivityProduct GetActivityProductById(int activityProductId)
        {
            return activityProductId == 0 ? null : _activityProductRepository.GetById(activityProductId);
        }

        public virtual List<Activity> GetActivitiesByPromotionAndProductId(int promotionId, int productId)
        {
            var query = from i in _activityProductRepository.Table where i.ProductId == productId && i.PromotionId == promotionId select i.Activty;
            return query.ToList();
        }

        public virtual void DeleteActivityProduct(ActivityProduct activityProduct)
        {
            if (activityProduct == null)
                throw new ArgumentNullException("activityProduct");
            var activity = activityProduct.Activty;
            if (activity == null) return;
            _activityProductRepository.Delete(activityProduct);
            //event notification
            _eventPublisher.EntityDeleted(activityProduct);
        }

        public virtual void UpdateActivityProduct(ActivityProduct activityProduct)
        {
            if (activityProduct == null)
                throw new ArgumentNullException("activityProduct");
            var activity = activityProduct.Activty;
            if (activity == null) return;
            _activityProductRepository.Update(activityProduct);
            //event notification
            _eventPublisher.EntityUpdated(activityProduct);
        }

        public virtual IPagedList<ActivityProductGroup> SearchActivityProductGroups(int activityId, int publishPlatform, int pageIndex, int pageSize)
        {
            var query = _activityProductGroupRepository.Table.Where(a => a.ActivityId == activityId);
            query = query.OrderBy(a => a.DisplayOrder);
            return new PagedList<ActivityProductGroup>(query, pageIndex, pageSize);
        }

        public virtual ActivityProductGroup GetActivityProductGroupById(int activityProductGroupId)
        {
            if (activityProductGroupId == 0)
                return null;

            return _activityProductGroupRepository.GetById(activityProductGroupId);
        }

        public virtual void InsertActivityProductGroup(ActivityProductGroup activityProductGroup)
        {
            if (activityProductGroup == null)
                throw new ArgumentNullException("activityProductGroup");

            _activityProductGroupRepository.Insert(activityProductGroup);
            //event notification
            _eventPublisher.EntityInserted(activityProductGroup);
        }

        public virtual void UpdateActivityProductGroup(ActivityProductGroup activityProductGroup)
        {
            if (activityProductGroup == null)
                throw new ArgumentNullException("activityProductGroup");

            _activityProductGroupRepository.Update(activityProductGroup);

            //event notification
            _eventPublisher.EntityUpdated(activityProductGroup);
        }

        public virtual void DeleteActivityProductGroup(ActivityProductGroup activityProductGroup)
        {
            if (activityProductGroup == null)
                throw new ArgumentNullException("activityProductGroup");

            _activityProductGroupRepository.Delete(activityProductGroup);
            //event notification
            _eventPublisher.EntityDeleted(activityProductGroup);
        }

        public ActivityStyle GetActivityStyleByActivityId(int activityId, int platForm)
        {
            var table = _activityStyleRepository.Table;
            var query = table.FirstOrDefault(a => a.ActivityId == activityId && a.Platform == platForm);
            return query;
        }

        public virtual void InsertActivityStyle(ActivityStyle activityStyle)
        {
            if (activityStyle == null)
                throw new ArgumentNullException("activityStyle");

            _activityStyleRepository.Insert(activityStyle);
            //event notification
            _eventPublisher.EntityInserted(activityStyle);
        }

        public virtual void UpdateActivityStyle(ActivityStyle activityStyle)
        {
            if (activityStyle == null)
                throw new ArgumentNullException("activityStyle");

            _activityStyleRepository.Update(activityStyle);

            //event notification
            _eventPublisher.EntityUpdated(activityStyle);
        }

        #region ActivityComment Members

        public IPagedList<ActivityComment> GetActivityComments(int actvityId, int pageSize = 20, int pageIndex = 0)
        {
            var table = _activityCommentRepository.Table;
            var query = table.Where(p => p.ActivityId == actvityId)
                .OrderByDescending(p => p.CreateOnUTC);
            return new PagedList<ActivityComment>(query, pageIndex, pageSize);
        }

        public bool AddActivityComment(int customerId, int activityId, string Content, int? ReplyCommentId = null)
        {

            ActivityComment activityComment = new ActivityComment();
            activityComment.CustomerId = customerId;
            activityComment.ActivityId = activityId;
            activityComment.AuditStatus = 1;
            activityComment.Deleted = false;
            activityComment.Content = Content;
            activityComment.CreateOnUTC = DateTime.UtcNow;
            if (ReplyCommentId != null && ReplyCommentId.HasValue)
            {
                activityComment.ReplyTargetId = ReplyCommentId.Value;
            }
            _activityCommentRepository.Insert(activityComment);

            return true;

        }

        public bool IsCommentCustomerPraised(int customerId, int commentId)
        {
            try
            {
                var table = _activityCommentRepository.Table;
                var comment = table.FirstOrDefault(p => p.Id == commentId);
                if (comment != null)
                {
                    var buffer = comment.PraiseLogs.Where(x => x.CustomerId == customerId);
                    if (buffer == null || buffer.Count() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取消对评论的赞
        /// </summary>
        /// <param name="customerId">用户ID</param>
        /// <param name="commentId">评论ID</param>
        /// <returns></returns>
        public bool DeleteCommentPraise(int customerId, int commentId)
        {
            var ispaised = _activityCommentPraiseRepository.Table.FirstOrDefault(p => p.CustomerId == customerId && p.CommentId == commentId);
            if (ispaised == null)
            {
                return false;
            }
            else
            {
                _activityCommentPraiseRepository.Delete(ispaised);
                return true;
            }
        }

        /// <summary>
        /// 对评论的赞
        /// </summary>
        /// <param name="customerId">用户ID</param>
        /// <param name="commentId">评论ID</param>
        /// <returns></returns>
        public bool AddCommentPraise(int customerId, int commentId)
        {
            var ispaised = _activityCommentPraiseRepository.Table.FirstOrDefault(p => p.CustomerId == customerId && p.CommentId == commentId);
            if (ispaised == null)
            {
                ActivityCommentPraise praise = new ActivityCommentPraise();
                praise.CustomerId = customerId;
                praise.CommentId = commentId;
                praise.CreateOnUTC = DateTime.UtcNow;
                _activityCommentPraiseRepository.Insert(praise);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetCommentSendIndex(int commentId)
        {
            var comment = _activityCommentRepository.Table.FirstOrDefault(P => P.Id == commentId);
            if (comment != null)
            {
                return comment.Activity.Comments.Where(p => p.CreateOnUTC < comment.CreateOnUTC).Count() + 1;
            }
            else
            {
                throw new Exception("评论不存在");
            }
        }

        #endregion

        #region IActivityService Members




        public IPagedList<ActivityProduct> GetActivityProductsMapping(int activityid, int pageIndex, int pageSize, ActivityPlatform platform)
        {
            var table = _activityProductRepository.Table;
            //zaylor_update [正式需更改]
            var query = table.Where(p => (p.Product.AvailableEndDateTimeUtc > DateTime.UtcNow || !p.Product.AvailableEndDateTimeUtc.HasValue)
                && p.Product.Published
                && (!p.Product.Deleted)
                && p.Promotion.EndDateTimeUtc > DateTime.UtcNow
                //&& (p.Product.StockQuantity - p.Product.MinStockQuantity) > 0
                && p.ActivityId == activityid
                && (p.Product.AvailableEndDateTimeUtc > DateTime.UtcNow || !p.Product.AvailableEndDateTimeUtc.HasValue)
                );
            switch (platform)
            {
                case ActivityPlatform.App:
                    query = query.Where(p => p.IsAppPlatformAvailable);
                    break;
                case ActivityPlatform.Pc:
                    query = query.Where(p => p.IsPcPlatformAvailable);
                    break;
                case ActivityPlatform.Wechat:
                    query = query.Where(p => p.IsWeixinPlatformAvailable);
                    break;
                default:
                    break;
            }
            query = query.OrderBy(p => p.Group == null ? 0 : p.Group.DisplayOrder)
               .ThenBy(p => p.ShowOnHomePage)
               .Select(p => p);

            return new PagedList<ActivityProduct>(query, pageIndex, pageSize);
        }

        #endregion

    }
}
