using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class SpecialSaleStageGroup : BaseEntity
    {
        private ICollection<SpecialSaleStage> _specialSaleStages;

        /// <summary>
        /// 卖场分组名称
        /// </summary>
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool Deleted { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public virtual ICollection<SpecialSaleStage> SpecialSaleStages
        {
            get
            {
                return _specialSaleStages ?? (_specialSaleStages = new List<SpecialSaleStage>());

            }
            set
            {
                _specialSaleStages = value;
            }

        }



    }
}
