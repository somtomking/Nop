using Nop.Core;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Plugin.Widgets.ProductSpecialSale.Models;
using Nop.Plugin.Widgets.ProductSpecialSale.Services;
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
using Nop.Web.Framework.Kendoui;
using Nop.Services.Stores;
using Nop.Core.Caching;
using AutoMapper;
namespace Nop.Plugin.Widgets.ProductSpecialSale.Controllers
{
    public class WidgetsProductSpecialSaleController : BasePluginController
    {
        private readonly static string _Widget_Path_Format = "~/Plugins/Widgets.ProductSpecialSale/Views/WidgetsProductSpecialSale/{0}.cshtml";
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ISpecialSaleStageService _specialSaleStageService;
        public WidgetsProductSpecialSaleController(
            IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ISpecialSaleStageService specialSaleStageService
            )
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._specialSaleStageService = specialSaleStageService;
        }
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


        [HttpPost]
        public ActionResult SpecialSaleStageList(DataSourceRequest command, SpecialSaleStageQueryModel model)
        {
            var result = _specialSaleStageService.QuerySpecialSaleStage(command.Page, command.PageSize);
            var modelData = new List<SpecialSaleStageModel>();
            foreach (var item in result)
            {
                var vm = new SpecialSaleStageModel();
                vm = AutoMapper.Mapper.Map(item, vm);
                modelData.Add(vm);
            }
            var data = new DataSourceResult()
            {
                Data = modelData,
                Total = result.Count(),
            };
            return Json(data);
        }
        #endregion


    }
}
