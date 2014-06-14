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
            return View(GetViewPath("Configure"), new SpecialSaleStageQueryModel());
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
            return ViewPathHelper.GetViewPath(viewName);
        }
        #endregion



        #region 特卖
        public ActionResult CreateStageGroup()
        {
            return View(GetViewPath("CreateStageGroup"), new SpecialSaleStageGroupModel());
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateStageGroup(SpecialSaleStageGroupModel model, bool continueEditing)
        {
            if (this.ModelState.IsValid)
            {
                var data = model.ToEntity();
                _specialSaleStageService.CreateSpecialSaleStageGroup(data);
                var vm = data.ToModel();
                vm.SaleGroupCreate.SpecialSaleStageGroupId = vm.Id;

                SuccessNotification("添加特卖分组成功!");
                return continueEditing ? RedirectToAction("EditStageGroup", new { id = vm.Id }) : RedirectToAction("List");
            }
            return View(GetViewPath("CreateStageGroup"), model);
        }

        public ActionResult EditStageGroup(int? id)
        {
            if (!id.HasValue)
            {
                return Content("Id is null");
            }
            var data = _specialSaleStageService.GetSpecialSaleStageGroupById(id.Value);
            if (data == null)
            {
                return Content(string.Format("未能找到分组:[ID:{0}]", id));
            }
            var model = data.ToModel();
            model.SaleGroupCreate.SpecialSaleStageGroupId = model.Id;

            return View(GetViewPath("CreateStageGroup"), model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditStageGroup(SpecialSaleStageGroupModel model, bool continueEditing)
        {
            if (this.ModelState.IsValid)
            {
                var data = model.ToEntity();
                _specialSaleStageService.UpdateSpecialSaleStageGroup(data);
                SuccessNotification("更新特卖分组成功!");
                return continueEditing ? RedirectToAction("EditStageGroup", new { id = model.Id }) : RedirectToAction("List");
            }


            return View(GetViewPath("CreateStageGroup"), model);
        }

        [HttpPost]
        public ActionResult SpecialSaleStageList(DataSourceRequest command, SpecialSaleStageQueryModel model)
        {
            var result = _specialSaleStageService.QuerySpecialSaleStage(command.Page-1, command.PageSize);

            var modelData = new List<SpecialSaleStageGroupModel>();
            foreach (var item in result)
            {
                var vm = new SpecialSaleStageGroupModel();
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
        [HttpPost]
        public ActionResult SpecialSaleGroupList(DataSourceRequest command, int saleStageId)
        {
            var modelData = new List<SpecialSaleStageModel>();
            var data = new DataSourceResult()
            {
                Data = modelData,
                Total = 1,
            };
            return Json(data);

        }
        #endregion


    }
}
