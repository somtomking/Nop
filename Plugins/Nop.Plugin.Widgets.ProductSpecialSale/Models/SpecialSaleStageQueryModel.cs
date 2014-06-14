using System.ComponentModel;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class SpecialSaleStageQueryModel : BaseNopModel
    {
        [DisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }
        [DisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
    }
}
