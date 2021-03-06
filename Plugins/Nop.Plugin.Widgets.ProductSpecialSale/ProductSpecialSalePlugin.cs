﻿using Nop.Core.Plugins;
using Nop.Services.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.ProductSpecialSale
{
    class ProductSpecialSalePlugin : BasePlugin, IWidgetPlugin
    {
        public IList<string> GetWidgetZones()
        {
            return new string[] { "home_page_productspecialsale" };
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsProductSpecialSale";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.ProductSpecialSale" }, { "area", null } };
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsProductSpecialSale";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.ProductSpecialSale"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        public override void Install()
        {
            base.Install();
        }
        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}
