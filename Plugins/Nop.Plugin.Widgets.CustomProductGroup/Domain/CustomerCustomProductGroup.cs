using Nop.Core;
using Nop.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Domain
{
    public partial class CustomerCustomProductGroup : BaseEntity
    {
        public int CustomerId { get; set; }

        public string CustomProductGroupIds { get; set; }

        //public Customer Customer { get; set; }
    }
}
