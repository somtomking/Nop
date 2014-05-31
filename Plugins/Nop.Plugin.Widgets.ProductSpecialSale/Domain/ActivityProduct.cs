using System;
using Nop.Core.Domain.Catalog;
using Nop.Core;


namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class ActivityProduct : BaseEntity
    {
        public int ActivityId { set; get; }
        public int ProductId { set; get; }
        public int? ActivityProductGroupId { set; get; }
        public int? PromotionId { set; get; }
        public bool IsPcPlatformAvailable { get; set; }
        public bool IsAppPlatformAvailable { get; set; }
        public bool IsWeixinPlatformAvailable { get; set; }
        public int DisplayOrder { set; get; }
        public bool ShowOnHomePage { set; get; }
        public virtual Activity Activty { set; get; }
        public virtual Product Product { set; get; }
        public virtual ActivityProductGroup Group { set; get; }
        public virtual Promotion Promotion { set; get; }
    }
} 