using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Controllers
{
    public class WidgetsProductSpecialSaleController : BasePluginController
    {
        private readonly static string _Widget_Path_Format = "~/Plugins/Widgets.ProductSpecialSale/Views/WidgetsProductSpecialSale/{0}.cshtml";
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return View(GetViewPath("Configure"));
        }
        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(string s)
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            
            return View(GetViewPath("PublicInfo"));
        }

        private string GetViewPath(string viewName)
        {
            return string.Format(_Widget_Path_Format, viewName);
        }
    }
}
