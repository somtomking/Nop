using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class SpecialSaleStageModel : BaseNopEntityModel
    {
        [DisplayName("特卖名称")]
        public string Name { get; set; }
        [DisplayName("特卖开始时间")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartTime { get; set; }
        [DisplayName("特卖结束时间")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndTime { get; set; }
        [DisplayName("特卖显示顺序")]
        [UIHint("Int32Nullable")]
        public int DisplayOrder { get; set; }
    }
}
