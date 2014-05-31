using Nop.Core;
namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public class ActivityStyle : BaseEntity
    {
        public int ActivityId { set; get; }
        public int Platform { set; get; }
        public int PosterPictureId { set; get; }
        public int? BackgroundPictureId { set; get; }
        public virtual Activity Activty { set; get; }
    }
}