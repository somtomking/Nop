using Nop.Core;
using System;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class PromotionTracking : BaseEntity
    {
        public int PromotionId { get; set; }
        public PromotionTrackingType Type { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Remark { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}