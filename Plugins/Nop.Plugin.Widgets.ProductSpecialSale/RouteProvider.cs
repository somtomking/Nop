using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.ProductSpecialSale
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.ProductSpecialSale.CreateStageGroup",
                 "Plugins/ProductSpecialSale/CreateStage",
                 new { controller = "WidgetsProductSpecialSale", action = "CreateStageGroup" },
                 new[] { "Nop.Plugin.Widgets.ProductSpecialSale.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.ProductSpecialSale.EditStageGroup",
                 "Plugins/ProductSpecialSale/EditStage",
                 new { controller = "WidgetsProductSpecialSale", action = "EditStageGroup" },
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
