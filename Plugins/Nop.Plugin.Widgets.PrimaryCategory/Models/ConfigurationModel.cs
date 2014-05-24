using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.PrimaryCategory.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }


        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order1 { get; set; }
        public bool Order1_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture1Id { get; set; }
        public bool Picture1Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link1 { get; set; }
        public bool Link1_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order2 { get; set; }
        public bool Order2_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture2Id { get; set; }
        public bool Picture2Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link2 { get; set; }
        public bool Link2_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order3 { get; set; }
        public bool Order3_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture3Id { get; set; }
        public bool Picture3Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link3 { get; set; }
        public bool Link3_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order4 { get; set; }
        public bool Order4_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture4Id { get; set; }
        public bool Picture4Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link4 { get; set; }
        public bool Link4_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order5 { get; set; }
        public bool Order5_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture5Id { get; set; }
        public bool Picture5Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link5 { get; set; }
        public bool Link5_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order6 { get; set; }
        public bool Order6_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture6Id { get; set; }
        public bool Picture6Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link6 { get; set; }
        public bool Link6_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order7 { get; set; }
        public bool Order7_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture7Id { get; set; }
        public bool Picture7Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link7 { get; set; }
        public bool Link7_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order8 { get; set; }
        public bool Order8_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture8Id { get; set; }
        public bool Picture8Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link8 { get; set; }
        public bool Link8_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Order")]
        [AllowHtml]
        public string Order9 { get; set; }
        public bool Order9_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Picture")]
        [UIHint("Picture")]
        public int Picture9Id { get; set; }
        public bool Picture9Id_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.Link")]
        [AllowHtml]
        public string Link9 { get; set; }
        public bool Link9_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.PrimaryCategory.ProductsToKill")]
        [AllowHtml]
        public string ProductIdsToKill { get; set; }
        public bool ProductIdsToKill_OverrideForStore { get; set; }
    }
}