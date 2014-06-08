using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class SpecialSaleGroupModel : BaseNopEntityModel
    {
        public string Name { get; set; }
        public int PictureId { get; set; }
        public int DisplayOrder { get; set; }
        public bool Enable { get; set; }


        public class SpecialSaleGroupCreateModel:SpecialSaleGroupModel
        {
            public int SpecialSaleStageId { get; set; }


        }
    }

   
}
