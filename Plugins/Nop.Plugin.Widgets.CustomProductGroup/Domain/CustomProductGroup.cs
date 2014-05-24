using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Domain
{
    public class CustomProductGroup : BaseEntity
    {
        private ICollection<CustomProductGroupItem> _customProductGroupItems;

        /// <summary>
        /// 商品主题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 商品主题风格
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 分类链接
        /// </summary>
        public string SubCategoryLinks { get; set; }

        /// <summary>
        /// "更多"链接
        /// </summary>
        public string MoreLink { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommended { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// APP模块图片
        /// </summary>
        public int? PictureId { get; set; }

        /// <summary>
        /// APP模块图片2
        /// </summary>
        public int? PictureId2 { get; set; }

        public int? FirstProductPictureId { get; set; }

        public bool IsSelected { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        public string AdLink { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        public int? AdLinkPictureId { get; set; }

        /// <summary>
        /// 显示平台
        /// </summary>
        public int Plantform { get; set; }

        public ICollection<CustomProductGroupItem> CustomProductGroupItems
        {
            get { return _customProductGroupItems ?? (_customProductGroupItems = new List<CustomProductGroupItem>()); }
            protected set { _customProductGroupItems = value; }
        }
    }
}
