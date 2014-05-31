using Nop.Core;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class ActivityProductGroup : BaseEntity
    {
        public int ActivityId { set; get; }
        public int Platform { set; get; }
        public string Name { set; get; }
        public string BackgroundColor { set; get; }
        public int DisplayOrder { set; get; }

        public virtual Activity Activty { set; get; }

        public virtual ICollection<ActivityProduct> Products { get; set; }
    }
}