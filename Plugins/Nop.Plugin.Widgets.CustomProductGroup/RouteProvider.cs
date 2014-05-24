using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.CustomProductGroup
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.CustomProductGroup.EditPopup",
                 "Plugins/CustomProductGroup/EditPopup",
                 new { controller = "WidgetsCustomProductGroup", action = "EditPopup" },
                 new[] { "Nop.Plugin.Widgets.CustomProductGroup.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.CustomProductGroup.EditProductPopup",
                 "Plugins/CustomProductGroup/EditProductPopup",
                 new { controller = "WidgetsCustomProductGroup", action = "EditProductPopup" },
                 new[] { "Nop.Plugin.Widgets.CustomProductGroup.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.CustomProductGroup.EditProductPopup2",
              "Plugins/CustomProductGroup/EditProductPopup2",
              new { controller = "WidgetsCustomProductGroup", action = "EditProductPopup2" },
              new[] { "Nop.Plugin.Widgets.CustomProductGroup.Controllers" }
         );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
