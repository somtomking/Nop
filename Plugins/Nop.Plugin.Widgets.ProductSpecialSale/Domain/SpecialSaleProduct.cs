using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    /// <summary>
    /// 特卖产品
    /// </summary>
    public class SpecialSaleProduct : BaseEntity
    {
        public int ProdcutId { get; set; }
        public double Price { get; set; }
        public double OriginalPrice { get; set; }
        public int DisplayOrder { get; set; }
        public int Quantity { get; set; }
        public SpecialSaleStage SpecialSaler { get; set; }

        public int SpecialSalerId { get; set; }
    }
}
