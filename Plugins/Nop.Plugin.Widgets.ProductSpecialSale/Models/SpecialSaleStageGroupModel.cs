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
    public class SpecialSaleStageGroupModel : BaseNopEntityModel
    {
        public SpecialSaleStageGroupModel()
        {
            SaleGroupCreate = new SpecialSaleStageModel.SpecialSaleStageCreateModel();
        }
        [DisplayName("特卖分组名称")]
        public string Name { get; set; }
        [DisplayName("特卖显示顺序")]
        [UIHint("Int32Nullable")]
        public int DisplayOrder { get; set; }
        [DisplayName("启用")]
        public bool Enable { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("最后更新时间")]
        public DateTime? LastUpdateTime { get; set; }

        public bool HasSaleStage { get; set; }
        public SpecialSaleStageModel.SpecialSaleStageCreateModel SaleGroupCreate { get; set; }


    }

}
