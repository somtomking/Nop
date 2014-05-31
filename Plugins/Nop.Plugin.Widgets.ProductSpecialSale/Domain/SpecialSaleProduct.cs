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
        public int Quantity { get; set; }
        public SpecialSaleGroup SpecialSaler { get; set; }
    }
}
