using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Controllers
{
    public class WidgetsProductSpecialSaleController : Controller
    {
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure() {
            return View("~/Plugins/Widgets.ProductSpecialSale/Views/WidgetsProductSpecialSale/Configure.cshtml");
        }
        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(string s)
        {
            return View();
        }
    }
}
