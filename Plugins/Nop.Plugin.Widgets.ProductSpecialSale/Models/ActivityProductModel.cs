
using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public partial class ActivityProductModel : BaseEntity
    {
        public ActivityProductModel()
        { }
        public int ActivityId { set; get; }
        public int Platform { set; get; }
        public int? ActivityProductGroupId { set; get; }
        public int ProductId { set; get; }
        public int PromotionId { set; get; }
        public DateTime StartTimeUtc { set; get; }
        public DateTime EndTimeUtc { set; get; }
        public int DisplayOrder { set; get; }
        public string Price { set; get; }
        public string PcPlatformPrice { set; get; }

        public bool IsSoldOut { get; set; }


        public Nop.Core.Domain.Catalog.Product Product { set; get; }
        public Nop.Core.Domain.Catalog.ProductPicture ProductPicture { set; get; }
        public string PictureUrl { set; get; }
    }
}