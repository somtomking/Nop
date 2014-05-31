using Nop.Core;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Plugin.Widgets.ProductSpecialSale.Models;
using Nop.Plugin.Widgets.ProductSpecialSale.Serivces;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nop.Services.Seo;
using System.Globalization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Controllers
{
    public class WidgetsProductSpecialSaleController : BasePluginController
    {
        private readonly static string _Widget_Path_Format = "~/Plugins/Widgets.ProductSpecialSale/Views/WidgetsProductSpecialSale/{0}.cshtml";

      
        #region 配置
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
        #endregion



        #region 特卖
        public ActionResult CreateStage()
        {
            return View(GetViewPath("CreateStage"));
        }

        public ActionResult EditStage()
        {
            return View(GetViewPath("EditStage"));
        }

        #endregion


    }
}
