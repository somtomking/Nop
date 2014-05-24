using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.PrimaryCategory.Models
{
    public partial class PublicInfoModel : BaseNopModel
    {
        public PublicInfoModel()
        {
            listSettings = new List<Setting>();
            Products = new List<ProductModel>();
        }

        public IList<Setting> listSettings { get; set; }
        public IList<ProductModel> Products { get; set; }
        
        public class Setting
        {
            public string Order { get; set; }
            public string PictureUrl { get; set; }
            public string Link { get; set; }
        }

        public partial class ProductModel : BaseNopModel
        {
            public string Name { get; set; }
            public string SeName { get; set; }
            public string ImageUrl { get; set; }
            public string Title { get; set; }
            public string AlternateText { get; set; }
            public string OldPrice { get; set; }
            public string Price { get; set; }
        }
    }
}