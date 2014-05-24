using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Plugin.Widgets.PrimaryCategory.Core;

namespace Nop.Plugin.Widgets.PrimaryCategory
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class PrimaryCategoryPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public PrimaryCategoryPlugin(IPictureService pictureService,
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
            return new List<string>() { "home_page_primary_category" };
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
            controllerName = "WidgetsPrimaryCategory";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.PrimaryCategory.Controllers" }, { "area", null } };
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
            controllerName = "WidgetsPrimaryCategory";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.PrimaryCategory.Controllers"},
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
            var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.PrimaryCategory/Content/sample/");

            //settings
            var settings = new PrimaryCategorySettings();
            for (var i = 1; i <= 9; i++)
            {
                var order = i.ToString();
                var pictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "class_" + i.ToString("00") + ".jpg"), "image/pjpeg", "banner_1", true).Id;
                var link = _webHelper.GetStoreLocation(false);
                ReflectionHelper.SetValue(settings, "Order" + i.ToString(), order);
                ReflectionHelper.SetValue(settings, "Picture" + i.ToString() + "Id", pictureId);
                ReflectionHelper.SetValue(settings, "Link" + i.ToString(), link);
            }
            _settingService.SaveSetting(settings);


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture1", "Picture 1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture2", "Picture 2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture3", "Picture 3");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture4", "Picture 4");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture5", "Picture 5");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture6", "Picture 6");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture7", "Picture 7");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture8", "Picture 8");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture9", "Picture 9");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture.Hint", "Upload picture.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Order", "Order");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Order.Hint", "Enter order for picture. Leave empty if don't want to set the order.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Link.Hint", "Enter URL. Leave empty if don't want this picture to be clickable.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.ProductsToKill", "Products to kill");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.PrimaryCategory.ProductsToKill.Hint", "Products to kill");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<PrimaryCategorySettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture3");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture4");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture5");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture6");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture7");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture8");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture9");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Order");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Order.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.Link.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.ProductsToKill");
            this.DeletePluginLocaleResource("Plugins.Widgets.PrimaryCategory.ProductsToKill.Hint");

            base.Uninstall();
        }

    }
}
