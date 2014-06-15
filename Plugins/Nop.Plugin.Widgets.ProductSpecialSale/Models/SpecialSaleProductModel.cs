using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class SpecialSaleProductModel : BaseNopEntityModel
    {
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public double Price { get; set; }
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
    }
}
