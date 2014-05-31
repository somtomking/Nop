using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class Promotion : BaseEntity
    {
        public string Name { get; set; }
        public PromotionType Type { get; set; }
        public PromotionStatus Status { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }
        public bool IsEnabled { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime LastUpdatedDateTimeUtc { get; set; }
        public bool Deleted { get; set; }
        public virtual IList<PromotionProduct> Products { get; set; }
        public virtual IList<PromotionTracking> Trackings { get; set; }
        public virtual IList<PromotionProductTracking> ProductTrackings { get; set; }
    }
}