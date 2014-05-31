using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class Activity : BaseEntity
    {
        public string Name { set; get; }
        public int Category { set; get; }
        public int Type { set; get; }
        public int Status { set; get; }
        public bool IsPcPlatformAvailable { set; get; }
        public bool IsAppPlatformAvailable { set; get; }
        public bool IsWeixinPlatformAvailable { set; get; }
        public DateTime? StartTimeUtc { set; get; }
        public DateTime? EndTimeUtc { set; get; }
        public string Description { set; get; }
        public DateTime CreatedOnUtc { set; get; }
        public DateTime LastUpdatedTimeUtc { set; get; }
        public bool Deleted { get; set; }
        public virtual ICollection<ActivityProduct> Products { set; get; }
        public virtual ICollection<ActivityProductGroup> Groups { set; get; }
        public virtual ICollection<ActivityStyle> Styles { set; get; }
        public virtual ICollection<ActivityComment> Comments { get; set; }

    }
}