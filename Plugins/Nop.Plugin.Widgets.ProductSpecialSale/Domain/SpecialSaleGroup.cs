using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    /// <summary>
    /// 特卖产品分组
    /// </summary>
    public class SpecialSaleGroup:BaseEntity
    {
        public string Name { get; set; }
        public int PictureId { get; set; }
        public int DisplayOrder { get; set; }
        public SpecialSaleStage SpecialSaleStage { get; set; }
        ICollection<SpecialSaleProduct> SpecialSaleProducts { get; set; }
    }

}
