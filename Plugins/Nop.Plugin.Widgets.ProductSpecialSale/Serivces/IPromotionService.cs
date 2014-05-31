using Nop.Core;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public interface IPromotionService
    {
        IList<Promotion> GetAllPromotions();
        IList<Promotion> GetProcessingPromotions();
        void StartPromotion(Promotion promotion);
        void CompletePromotion(Promotion promotion);
        IPagedList<Promotion> SearchPromotions(string billNumber, string billName, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc, PromotionType? promotionType, PromotionStatus? promotionStatus, bool? isEnabled, int pageIndex, int pageSize);
        Promotion GetPromotionById(int promotionId);
        void UpdatePromotion(Promotion promotion);
        PromotionProduct GetPromotionProductById(int promotionProductId);
        void DeletePromotionProduct(PromotionProduct promotionProduct);
        void DeletePromotion(Promotion promotion);
        void UpdatePromotionProduct(PromotionProduct promotionProduct);
        IPagedList<PromotionProduct> GetPromotionProductsByPromotionId(int promotionId, string searchCondition, int pageIndex, int pageSize);
        void InsertPromotionProduct(PromotionProduct promotionProduct);
        void InsertPromotion(Promotion promotion);

        /// <summary>
        /// 根据商品ID搜索该商品正处在那个促销单中
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Promotion GetPromotionByProductId(int productId);

        IList<PromotionProduct> GetPromotionProductsByPromotionDate(DateTime startDateTimeUtc, DateTime endDateTimeUtc);
    }
}
