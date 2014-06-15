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
        [DisplayName("描述")]
        public string ShortDescription { get; set; }
        [DisplayName("特卖开始时间")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartTime { get; set; }
        [DisplayName("特卖结束时间")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndTime { get; set; }
        [DisplayName("商品图片")]
        [UIHint("Picture")]
        public int PictureId { get; set; }
        [DisplayName("排序")]
        [UIHint("Int32Nullable")]
        public int DisplayOrder { get; set; }
        [DisplayName("启用")]
        public bool Enable { get; set; }


        public class SpecialSaleStageCreateModel : SpecialSaleStageModel
        {
            public int SpecialSaleStageGroupId { get; set; }


        }
        public class SpecialSaleStageListModel : SpecialSaleStageModel
        {
            public string ImagePath { get; set; }
            public string SourceImagePath { get; set; }


        }
    }


}
