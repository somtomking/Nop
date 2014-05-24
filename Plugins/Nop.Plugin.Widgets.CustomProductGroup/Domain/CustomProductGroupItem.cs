using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Domain
{
    public class CustomProductGroupItem : BaseEntity
    {
        /// <summary>
        /// 商品主题Id
        /// </summary>
        public int CustomProductGroupId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int? PictureId { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 显示平台
        /// </summary>
        public int Plantform { get; set; }

        public CustomProductGroup CustomProductGroup { get; set; }

        public Product Product { get; set; }
    }
}
