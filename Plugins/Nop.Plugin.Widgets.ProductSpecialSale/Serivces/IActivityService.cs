using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public partial interface IActivityService
    {
        /// <summary>
        /// 根据id获取活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
       Activity GetActivityById(int activityId);

        /// <summary>
        /// 根据类型活动获取活动信息
        /// </summary>
        /// <param name="activityType">限时抢购活动</param>
        /// <returns></returns>
        Activity GetActivityByType(int activityType);

        IPagedList<Product> GetActivityProducts(int activityid, int pageSize, int pageIndex);

        /// <summary>
        /// 根据活动类型,获取对应的活动列表.[author：Zaylor，date:2014-3-10]
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <param name="isWeiXin">是否是微信使用</param>
        /// <returns>活动列表</returns>
        IList<ActivityProduct> GetProductByActivityType(ActivityType activityType, bool isWeiXin);

        IList<Nop.Plugin.Widgets.ProductSpecialSale.Domain.Activity> GetActivityByAppPlatform();
        /// <summary>
        /// 获取活动列表.[author：Zaylor，date:2014-3-11]
        /// </summary>
        /// <param name="num">前面几条活动信息</param>
        /// <returns>活动列表</returns>
        IList<Activity> GetActivityTop(int num);

        /// <summary>
        /// 获取活动评论列表.[author：Craig，date:2014-3-12]
        /// </summary>
        /// <param name="actvityId">活动ID</param>
        /// <param name="pageSize">每页返回数量</param>
        /// <param name="pageIndex">页编码</param>
        /// <returns></returns>
        IPagedList<ActivityComment> GetActivityComments(int actvityId,int pageSize=20,int pageIndex=0);

        /// <summary>
        /// 添加活动评论[author：Craig，date:2014-3-12]
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="Content">评论内容</param>
        /// <param name="ReplyCommentId">被回复的评论ID</param>
        /// <returns>评论是否成功</returns>
        bool AddActivityComment(int customerId, int activityId, string Content, int? ReplyCommentId = null);

        /// <summary>
        /// 判断某条评论是否被制定用户赞过
        /// </summary>
        /// <param name="customerId">用户ID</param>
        /// <param name="commentId">评论ID</param>
        /// <returns></returns>
        bool IsCommentCustomerPraised(int customerId, int commentId);

        bool DeleteCommentPraise(int customerId, int commentId);

        bool AddCommentPraise(int customerId, int commentId);

        int GetCommentSendIndex(int commentId);

        IList<ActivityProduct> GetActivityProductsByProductId(int productId, bool isPc = true);
        IList<ActivityProduct> GetActivityProductsByProductId(int productId,int activityId, bool isPc = true);
        IPagedList<Core.Domain.Catalog.Product> GetTopActivityProducts(DateTime? datetime, int pageSize, int pageIndex);

        IPagedList<ActivityProduct> GetActivityProductsMapping(int GroupId, int pageIndex, int pageSize, ActivityPlatform platform);
        IPagedList<Activity> SearchActivities(string activityName,int? activityCategory,int? activityType,int? activityStatus,bool? isPcPlatformAvailable,bool? isAppPlatformAvailable,bool? isWeixinPlatformAvailable, int pageIndex, int pageSize);

        IPagedList<ActivityProduct> GetActivityProductsByActivityId(int activityId,int? activityProductGroupId,
            string searchCondition, int pageIndex, int pageSize);

        void InsertActivityProduct(ActivityProduct activityProduct);
        void DeleteActivity(Activity activity);
        ActivityProduct GetActivityProductById(int activityProductId);
        void DeleteActivityProduct(ActivityProduct activityProduct);
        void UpdateActivityProduct(ActivityProduct activityProduct);
        void UpdateActivity(Activity activity);
        void InsertActivity(Activity activity);
        IPagedList<ActivityProductGroup> SearchActivityProductGroups(int activityId, int publishPlatform, int pageIndex, int pageSize);
        void InsertActivityProductGroup(ActivityProductGroup activityProductGroup);
        void DeleteActivityProductGroup(ActivityProductGroup activityProductGroup);
        ActivityProductGroup GetActivityProductGroupById(int activityProductGroupId);
        void UpdateActivityProductGroup(ActivityProductGroup activityProductGroup);
        ActivityStyle GetActivityStyleByActivityId(int activityId, int platForm);
        void InsertActivityStyle(ActivityStyle activityStyle);
        void UpdateActivityStyle(ActivityStyle activityStyle);
        List<Activity> GetActivitiesByPromotionAndProductId(int promotionId,int productId);
    }
}
