using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public partial class ActivityStyleModel : BaseNopModel
    {
        public int Id { set; get; }
        public int ActivityId { set; get; }
        public int Platform { set; get; }
        public int PosterPictureId { set; get; }
        public int BackgroundPictureId { set; get; }
    }
}