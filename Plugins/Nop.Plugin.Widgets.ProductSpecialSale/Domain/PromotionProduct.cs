using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class PromotionProduct : BaseEntity
    {
        public int PromotionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PcPlatformPrice { get; set; }
        public decimal AppPlatformPrice { get; set; }
        public decimal WeixinPlatformPrice { get; set; }
        public int? PcPlatformCustomerMaximumQuantity { get; set; }
        public int? AppPlatformCustomerMaximumQuantity { get; set; }
        public int? WeixinPlatformCustomerMaximumQuantity { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual Product Product { get; set; }
    }
}