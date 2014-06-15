using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    /// <summary>
    /// 卖场
    /// </summary>
    public class SpecialSaleStage : BaseEntity
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PictureId { get; set; }
        public int DisplayOrder { get; set; }
        public int SaleStageGroupId { get; set; }
        /// <summary>
        /// 所在的卖场分组
        /// </summary>
        public SpecialSaleStageGroup SpecialSaleStageGroup { get; set; }
        public virtual ICollection<SpecialSaleProduct> SpecialSaleProducts { get; set; }

        
    }

}
