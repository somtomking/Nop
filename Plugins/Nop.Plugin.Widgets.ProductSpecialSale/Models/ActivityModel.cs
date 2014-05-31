using FluentValidation.Attributes;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public partial class ActivityModel : BaseNopModel
    {
        public ActivityModel()
        {
            ActivityProductGroupList = new List<ActivityProductGroupModel>();
            ActivityProductList = new List<ActivityProductModel>();
            ActivityStyle = new ActivityStyle();
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public int Category { set; get; }
        public int Type { set; get; }
        public int State { set; get; }
        public DateTime? StartTimeUtc { set; get; }
        public DateTime? EndTimeUtc { set; get; }
        public string Description { set; get; }
        public DateTime CreatedOnUtc { set; get; }
        public DateTime LastUpdatedTimeUtc { set; get; }
        public bool HaveGroupedProducts { get; set; }
        public bool HaveNotGroupedProducts { get; set; }
        public string PosterPictureUrl { set; get; }
        public string BackgroundPictureUrl { set; get; }

        public ActivityStyle ActivityStyle { set; get; }
        public IList<ActivityProductGroupModel> ActivityProductGroupList { set; get; }
        public IList<ActivityProductModel> ActivityProductList { set; get; }
    }
}