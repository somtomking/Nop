using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class ActivityProductGroupModel
    {
        public ActivityProductGroupModel()
        {
            ActivityProductList = new List<ActivityProductModel>();
        }
        public int Id { set; get; }
        public int ActivityId { set; get; }
        public int Platform { set; get; }
        public string Name { set; get; }
        public string BackgroundColor { set; get; }
        public int OrderNumber { set; get; }
        public IList<ActivityProductModel> ActivityProductList { set; get; }
    }
}