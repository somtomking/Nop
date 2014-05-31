using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class ActivityComment : BaseEntity
    {
        public int CustomerId { get; set; }
        public string Content { get; set; }
        public bool Deleted { get; set; }
        public int AuditStatus { get; set; }
        public DateTime CreateOnUTC { get; set; }
        public int? ReplyTargetId { get; set; }
        public int ActivityId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual ActivityComment SourceComment { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<ActivityCommentPraise> PraiseLogs { get; set; }
    }
}
