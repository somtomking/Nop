using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.ProductSpecialSale
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.ProductSpecialSale.CreateStage",
                 "Plugins/ProductSpecialSale/CreateStage",
                 new { controller = "WidgetsProductSpecialSale", action = "CreateStage" },
                 new[] { "Nop.Plugin.Widgets.ProductSpecialSale.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.ProductSpecialSale.EditStage",
                 "Plugins/ProductSpecialSale/EditStage",
                 new { controller = "WidgetsProductSpecialSale", action = "EditStage" },
                 new[] { "Nop.Plugin.Widgets.ProductSpecialSale.Controllers" }
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
