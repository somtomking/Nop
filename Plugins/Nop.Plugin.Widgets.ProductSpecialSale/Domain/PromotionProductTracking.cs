using System;
using Nop.Core.Domain.Catalog;
using Nop.Core;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class PromotionProductTracking : BaseEntity
    {
        public int PromotionId { get; set; }
        public int ProductId { get; set; }
        public decimal SpecialPrice { get; set; }
        public decimal? OriginalSpecialPrice { get; set; }
        public DateTime SpecialPriceStartDateTimeUtc { get; set; }
        public DateTime? OriginalSpecialPriceStartDateTimeUtc { get; set; }
        public DateTime SpecialPriceEndDateTimeUtc { get; set; }
        public DateTime? OriginalSpecialPriceEndDateTimeUtc { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public int OriginalManageInventoryMethodId { get; set; }
        public int MinStockQuantity { get; set; }
        public int OriginalMinStockQuantity { get; set; }
        public int LowStockActivityId { get; set; }
        public int OriginalLowStockActivityId { get; set; }
        public int? CustomerMaximumQuantity { get; set; }
        public int? OriginalCustomerMaximumQuantity { get; set; }
        public bool OriginalPublished { get; set; }
        public bool OriginalDisableBuyButton { get; set; }
        public bool OriginalDisableWishlistButton { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual Product Product { get; set; }
    }
}