using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class ActivityCommentPraise : BaseEntity
    {
        public int CustomerId { get; set; }
        public int CommentId { get; set; }
        public DateTime CreateOnUTC { get; set; }
        
        public virtual Customer Customer{get;set;}
        public virtual ActivityComment ActivityComment { get; set; }

        
    }
}
