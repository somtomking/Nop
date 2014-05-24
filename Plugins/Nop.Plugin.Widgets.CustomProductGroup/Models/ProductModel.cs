using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    public class ProductModel : BaseNopModel
    {
        public ProductModel()
        {
            PictureUrl = "";
            ProductPrice = new ProductDetailsModel.ProductPriceModel();
        }

        public int CustomProductGroupItemId { get; set; }

        [Required]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.ProductId")]
        public int ProductId { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.PictureId")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.ProductId")]
        public string PictureUrl { get; set; }

        [Required]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Plantform")]
        [UIHint("PlantformType")]
        public int Plantform { get; set; }
     
        
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Plantform")]
        [UIHint("PlantformType")]
        public string PlantformName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.ProductName")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Sku")]
        public string Sku { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Price")]
        public decimal Price { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Published")]
        public bool Published { get; set; }

        //售罄
        public bool IsSoldOut { get; set; }

        public decimal OldPrice { get; set; }

        public ProductDetailsModel.ProductPriceModel ProductPrice { get; set; }
    }

    public class ProductSimpleModel : BaseNopModel
    {

        public int CustomProductGroupItemId { get; set; }
        [Required]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.ProductId")]
        public int ProductId { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.ProductName")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Plantform")]
        [UIHint("PlantformType")]
        public int PlantformVal { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Plantform")]
        [UIHint("PlantformType")]
        public string PlantformName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Sku")]
        public string Sku { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Price")]
        public decimal Price { get; set; }

        [Required]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.DisplayOrder")]
        public int DisplayOrderVal { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.Published")]
        public bool Published { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Product.SeName")]
        public string SeName { get; set; }

    }
}