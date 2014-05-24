using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    public class ProductAddModel : Admin.Models.Catalog.ProductModel.AddRelatedProductModel
    {
        public ProductAddModel()
            : base()
        {

        }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Plantform")]
        public int Plantform { get; set; }

    }
}
