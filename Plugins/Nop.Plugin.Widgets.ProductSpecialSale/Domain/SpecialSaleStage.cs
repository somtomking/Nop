using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class SpecialSaleStage : BaseEntity
    {
        /// <summary>
        /// 卖场名称
        /// </summary>
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DisplayOrder { get; set; }
        public bool Deleted { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public ICollection<SpecialSaleGroup> SpecialSaleGroups { get; set; }



    }
}
