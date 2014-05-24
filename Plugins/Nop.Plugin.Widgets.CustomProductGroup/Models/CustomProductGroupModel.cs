using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using Nop.Web.Framework;
using Nop.Core.Domain.Catalog;
using Nop.Web.Models.Catalog;
using FluentValidation.Attributes;
using Nop.Plugin.Widgets.CustomProductGroup.Validators;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    [Validator(typeof(CustomProductGroupValidator))]
    public class CustomProductGroupModel
    {
        public CustomProductGroupModel()
        {
            //this.Products = new List<ProductOverviewModel>();
            this.Products = new List<Product>();
            this.ProductIds = new List<int>();
        }

        public int Id { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Style")]
        public string Style { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Style")]
        public string StyleName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.SubCategoryLinks")]
        public string SubCategoryLinks { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.MoreLink")]
        public string MoreLink { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.IsEnable")]
        public bool IsEnable { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.IsRecommended")]
        public bool IsRecommended { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.PictureId")]
        public int PictureId { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.FirstProductPictureId")]
        public int FirstProductPictureId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.FirstProductPictureId")]
        public string FirstProductPictureUrl { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.AdLinkPictureId")]
        public int AdLinkPictureId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.AdLinkPictureId")]
        public string AdLinkPictureUrl { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.AdLink")]
        public string AdLink { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Plantform")]
        public int Plantform { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.Plantform")]
        public string PlantformName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.ShowAd")]
        public bool ShowAd
        {
            get
            {
                return this.AdLinkPictureId != 0;
            }
        }

        //public List<ProductOverviewModel> Products { get; set; }
        public List<Product> Products { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomProductGroup.ProductsCount")]
        public int ProductsCount { get; set; }

        public List<int> ProductIds { get; set; }

        public ProductModel FirstProduct { get; set; }

        public List<CategoryLink> CategoryLinks
        {
            get
            {
                var categoryLinks = new List<CategoryLink>();

                if (!string.IsNullOrEmpty(this.SubCategoryLinks))
                {
                    var linkTexts = this.SubCategoryLinks.Split(new string[] { "\r\n", "\\r\\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var linkText in linkTexts)
                    {
                        var titleAndLink = linkText.Split('|');
                        if (titleAndLink.Length == 2)
                            categoryLinks.Add(new CategoryLink { Title = titleAndLink[0], Link = titleAndLink[1] });
                    }
                }

                return categoryLinks;
            }
        }

        public class CategoryLink
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }

        public static List<SelectListItem> Styles = new List<SelectListItem>
    {
        new SelectListItem { Text = "橙色", Value = "indexBox-01" },
        new SelectListItem { Text = "紫色", Value = "indexBox-02" },
        new SelectListItem { Text = "蓝色", Value = "indexBox-03" }, 
        new SelectListItem { Text = "草绿色", Value = "indexBox-04" }, 
        new SelectListItem { Text = "玫红色", Value = "indexBox-05" }, 
        new SelectListItem { Text = "湖蓝色", Value = "indexBox-06" }, 
        new SelectListItem { Text = "棕红色", Value = "indexBox-07" }, 
        new SelectListItem { Text = "紫红色", Value = "indexBox-08" }, 
        new SelectListItem { Text = "墨绿色", Value = "indexBox-09" }, 
        new SelectListItem { Text = "灰蓝色", Value = "indexBox-10" } 
    };

    }
}
