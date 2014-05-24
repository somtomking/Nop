using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.FocusArea
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class FocusAreaPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public FocusAreaPlugin(IPictureService pictureService, 
            ISettingService settingService, IWebHelper webHelper)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "home_page_focus_area" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsFocusArea";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.FocusArea.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsFocusArea";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.FocusArea.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //pictures
            var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.FocusArea/Content/sample/");


            //settings
            var settings = new FocusAreaSettings()
            {
                BoxPictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "ad_01.jpg"), "image/pjpeg", "banner_1", true).Id,
                BoxLink = _webHelper.GetStoreLocation(false),
                Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner_01.jpg"), "image/pjpeg", "banner_1", true).Id,
                Text1 = "",
                Link1 = _webHelper.GetStoreLocation(false),
                Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner_02.jpg"), "image/pjpeg", "banner_2", true).Id,
                Text2 = "",
                Link2 = _webHelper.GetStoreLocation(false),
                Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner_03.jpg"), "image/pjpeg", "banner_3", true).Id,
                Text3 = "",
                Link3 = _webHelper.GetStoreLocation(false),
                //Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), "image/pjpeg", "banner_4", true).Id,
                //Text4 = "",
                //Link4 = _webHelper.GetStoreLocation(false),
            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.BoxPicture", "Box Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.BoxLink", "Box Link");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture1", "Picture 1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture2", "Picture 2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture3", "Picture 3");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture4", "Picture 4");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Picture.Hint", "Upload picture.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Text", "Comment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Text.Hint", "Enter comment for picture. Leave empty if don't want to display any text.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FocusArea.Link.Hint", "Enter URL. Leave empty if don't want this picture to be clickable.");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FocusAreaSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.BoxPicture");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.BoxLink");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture3");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture4");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Text.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.FocusArea.Link.Hint");
            
            base.Uninstall();
        }
    }
}
