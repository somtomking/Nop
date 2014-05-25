using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using Nop.Core;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;
using Nop.Web.Infrastructure.Cache;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Services.Catalog;
using Nop.Services.Seo;
using Nop.Services.Common;
using Nop.Services.Security;
using Nop.Services.Localization;
using Nop.Services.Vendors;
using Nop.Services.Tax;
using Nop.Services.Directory;

using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Media;

using Nop.Core.Domain.Common;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;

using Nop.Plugin.Widgets.CustomProductGroup.Models;
using Nop.Plugin.Widgets.CustomProductGroup.Domain;
using G = Nop.Plugin.Widgets.CustomProductGroup.Domain;
using Nop.Plugin.Widgets.CustomProductGroup.Services;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomProductGroup.Controllers
{
    public class WidgetsCustomProductGroupController : Controller
    {
        #region Constants
        private const string CUSTOMPRODUCTGROUP_PUBLICINFO_KEY = "Nop.widgets.customproductgroup.publicinfo";
        #endregion

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ICategoryService _categoryService;
        private readonly IVendorService _vendorService;
        private readonly IManufacturerService _manufacturerService;

        private readonly CatalogSettings _catalogSettings;
        private readonly ITaxService _taxService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;

        private readonly ICacheManager _cacheManager;
        private readonly MediaSettings _mediaSettings;
        private readonly IWebHelper _webHelper;
        private readonly ICustomProductGroupService _customProductGroupService;
        #endregion

        #region Ctor

        public WidgetsCustomProductGroupController(IWorkContext workContext,
            IStoreContext storeContext, IStoreService storeService,
            IPictureService pictureService, ISettingService settingService,
            IProductService productService, IPermissionService permissionService,
            ILocalizationService localizationService, ICategoryService categoryService,
            IVendorService vendorService, IManufacturerService manufacturerService,
            CatalogSettings catalogSettings, ITaxService taxService,
            IPriceCalculationService priceCalculationService, ICurrencyService currencyService,
            IPriceFormatter priceFormatter
            , ICustomProductGroupService customProductGroupService
            ,
            ICacheManager cacheManager, MediaSettings mediaSettings,
            IWebHelper webHelper)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._productService = productService;
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._categoryService = categoryService;
            this._vendorService = vendorService;
            this._manufacturerService = manufacturerService;
            this._catalogSettings = catalogSettings;
            this._taxService = taxService;
            this._priceCalculationService = priceCalculationService;
            this._currencyService = currencyService;
            this._priceFormatter = priceFormatter;
            this._customProductGroupService = customProductGroupService;
            this._cacheManager = cacheManager;
            this._mediaSettings = mediaSettings;
            this._webHelper = webHelper;
        }

        #endregion

        #region CustomProductGroup

        [HttpPost]
        public ActionResult CustomProductGroupList(DataSourceRequest command,CustomProductGroupListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var records = _customProductGroupService.GetAllCustomProductGroups(onlyShowEnable: model.OnlyShowEnable, plantformType: model.Plantform).ToList();
            var sbwModel = records.Select(cpg =>
            {
                var cpgm = new Models.CustomProductGroupModel()
                {
                    Id = cpg.Id,
                    Title = cpg.Title,
                    Style = cpg.Style,
                    IsEnable = cpg.IsEnable,
                    IsRecommended = cpg.IsRecommended,
                    DisplayOrder = cpg.DisplayOrder,
                    Plantform = cpg.Plantform,
                    ProductsCount = _customProductGroupService.GetCustomProductGroupItems(cpg.Id, Int32.MaxValue, PlantformType.Default).Count(),
                    AdLink = cpg.AdLink,
                };

                if (cpg.AdLinkPictureId.HasValue)
                    cpgm.AdLinkPictureId = cpg.AdLinkPictureId.Value;

                if (cpgm.Plantform == (int)PlantformType.Default)
                    cpgm.PlantformName = "所有平台";
                else if (cpgm.Plantform == (int)PlantformType.APP)
                    cpgm.PlantformName = "APP";
                else if (cpgm.Plantform == (int)PlantformType.PC)
                    cpgm.PlantformName = "PC";

                var styleItem = CustomProductGroupModel.Styles.FirstOrDefault(n => n.Value == cpgm.Style);
                cpgm.StyleName = styleItem == null ? cpgm.Style : styleItem.Text;

                return cpgm;

            }).ToList();


            var gridModel = new DataSourceResult
            {
                Data = sbwModel,
                Total = records.Count
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult CustomProductGroupDelete(int id, CustomProductGroupListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var cpg = _customProductGroupService.GetCustomProductGroupById(id);
            if (cpg != null)
            {
                _customProductGroupService.DeleteCustomProductGroupItems(cpg.Id);
                _customProductGroupService.DeleteCustomProductGroup(cpg);
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductsList(int CustomProductGroupId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            return PrepareProductListResult(CustomProductGroupId);
        }


        public ActionResult ProductUpdate(ProductSimpleModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");
            var cpgi = _customProductGroupService.GetCustomProductGroupItemById(model.CustomProductGroupItemId);
            cpgi.Plantform = model.PlantformVal;
            cpgi.DisplayOrder = model.DisplayOrderVal;
            _customProductGroupService.UpdateCustomProductGroupItem(cpgi);
            return PrepareProductListResult(cpgi.CustomProductGroupId);

        }

        [NonAction]
        public JsonResult PrepareProductListResult(int customProductGroupId)
        {
            var customProductGroupItems = _customProductGroupService.GetCustomProductGroupItems(customProductGroupId, 10000, PlantformType.Default).ToList();

            var products = new List<ProductSimpleModel>();
            foreach (var customProductGroupItem in customProductGroupItems)
            {
                var productEntity = _productService.GetProductById(customProductGroupItem.ProductId);
                if (productEntity == null)
                    continue;

                var m = new Models.ProductSimpleModel()
                {
                    CustomProductGroupItemId = customProductGroupItem.Id,
                    ProductId = productEntity.Id,
                    ProductName = productEntity.Name,
                    SeName = productEntity.GetSeName(),
                    Sku = productEntity.Sku,
                    Price = productEntity.Price,
                    DisplayOrderVal = customProductGroupItem.DisplayOrder,
                    Deleted = productEntity.Deleted,
                    Published = productEntity.Published,
                    PlantformVal = customProductGroupItem.Plantform,
                };

                if (m.PlantformVal == (int)PlantformType.Default)
                    m.PlantformName = "所有平台";
                else if (m.PlantformVal == (int)PlantformType.APP)
                    m.PlantformName = "APP";
                else if (m.PlantformVal == (int)PlantformType.PC)
                    m.PlantformName = "PC";

                //if (customProductGroupItem.PictureId.HasValue)
                //    m.PictureUrl = _pictureService.GetPictureUrl(customProductGroupItem.PictureId.Value, showDefaultPicture: false);

                products.Add(m);
            }
      
            var data = new DataSourceResult
            {
                Data = products,
                Total = products.Count
            };
            return Json(data);
        }


        public ActionResult ProductDelete(int customProductGroupItemId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var cpgi = _customProductGroupService.GetCustomProductGroupItemById(customProductGroupItemId);
            if (cpgi != null)
                _customProductGroupService.DeleteCustomProductGroupItem(cpgi);

            return PrepareProductListResult(cpgi.CustomProductGroupId);
        }

        public ActionResult EditPopup(int? id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            ViewBag.RefreshPage = false;

            var model = new CustomProductGroupModel();
            if (id.HasValue && id != 0)
            {
                model = PrepareCustomProductGroupModel(id.Value);
                if (model == null)
                    //No record found with the specified id
                    return RedirectToAction("Configure");
            }

            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditPopup", model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult EditPopup(bool continueEditing, CustomProductGroupModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            ViewBag.RefreshPage = true;
            ViewBag.CloseWindow = !continueEditing;

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var cpg = new G.CustomProductGroup()
                    {
                        Title = model.Title,
                        Style = model.Style,
                        SubCategoryLinks = model.SubCategoryLinks,
                        MoreLink = model.MoreLink,
                        IsEnable = model.IsEnable,
                        IsRecommended = model.IsRecommended,
                        DisplayOrder = model.DisplayOrder,
                        Plantform = model.Plantform,
                        PictureId = model.PictureId,
                        FirstProductPictureId = model.FirstProductPictureId,
                        AdLink = model.AdLink,
                        AdLinkPictureId = model.AdLinkPictureId
                    };
                    _customProductGroupService.InsertCustomProductGroup(cpg);
                    ModelState.Clear();
                    model = PrepareCustomProductGroupModel(cpg.Id);
                    return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditPopup", model);
                }
                else
                {
                    var cpg = _customProductGroupService.GetCustomProductGroupById(model.Id);
                    if (cpg == null)
                        //No record found with the specified id
                        return RedirectToAction("Configure");

                    cpg.Title = model.Title;
                    cpg.Style = model.Style;
                    cpg.SubCategoryLinks = model.SubCategoryLinks;
                    cpg.MoreLink = model.MoreLink;
                    cpg.IsEnable = model.IsEnable;
                    cpg.IsRecommended = model.IsRecommended;
                    cpg.DisplayOrder = model.DisplayOrder;
                    cpg.Plantform = model.Plantform;
                    cpg.PictureId = model.PictureId;
                    cpg.FirstProductPictureId = model.FirstProductPictureId;
                    cpg.AdLink = model.AdLink;
                    cpg.AdLinkPictureId = model.AdLinkPictureId;

                    _customProductGroupService.UpdateCustomProductGroup(cpg);
                    ModelState.Clear();
                    return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditPopup", model);
                }
            }

            ViewBag.RefreshPage = false;
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditPopup", model);
        }

        [NonAction]
        public CustomProductGroupModel PrepareCustomProductGroupModel(int customProductGroupId)
        {
            var cpg = _customProductGroupService.GetCustomProductGroupById(customProductGroupId);
            if (cpg == null)
                return null;

            var model = new CustomProductGroupModel()
            {
                Id = cpg.Id,
                Title = cpg.Title,
                Style = cpg.Style,
                SubCategoryLinks = cpg.SubCategoryLinks,
                MoreLink = cpg.MoreLink,
                IsEnable = cpg.IsEnable,
                IsRecommended = cpg.IsRecommended,
                DisplayOrder = cpg.DisplayOrder,
                Plantform = cpg.Plantform,
                AdLink = cpg.AdLink,
            };

            if (cpg.PictureId.HasValue)
                model.PictureId = cpg.PictureId.Value;

            if (cpg.FirstProductPictureId.HasValue)
                model.FirstProductPictureId = cpg.FirstProductPictureId.Value;

            if (cpg.AdLinkPictureId.HasValue)
                model.AdLinkPictureId = cpg.AdLinkPictureId.Value;

            return model;
        }

        #endregion

        #region CustomProductGroupItem

        public ActionResult EditProductPopup(int? customProductGroupItemId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var model = new ProductModel();
            if (customProductGroupItemId.HasValue)
            {
                var cpgi = _customProductGroupService.GetCustomProductGroupItemById(customProductGroupItemId.Value);
                if (cpgi == null)
                    //No record found with the specified id
                    return RedirectToAction("Configure");

                model = new ProductModel()
               {
                   CustomProductGroupItemId = cpgi.Id,
                   ProductId = cpgi.ProductId,
                   DisplayOrder = cpgi.DisplayOrder,
                   Plantform = cpgi.Plantform
               };

                if (cpgi.PictureId.HasValue)
                    model.PictureId = cpgi.PictureId.Value;

                var product = _productService.GetProductById(cpgi.ProductId);
                if (product != null)
                {
                    model.ProductName = product.Name;
                    model.SeName = product.GetSeName();
                }
            }

            ViewBag.RefreshPage = false;
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditProductPopup", model);
        }

        [HttpPost]
        public ActionResult EditProductPopup(int? customProductGroupId, ProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            //_productService.GetProductBySku();
            var product = _productService.GetProductById(model.ProductId);
            if (product == null)
                ModelState.AddModelError("", "商品不存在");

            if (ModelState.IsValid)
            {
                if (model.CustomProductGroupItemId == 0)
                {
                    var customProductGroupItem = new G.CustomProductGroupItem()
                    {
                        CustomProductGroupId = customProductGroupId.Value,
                        ProductId = model.ProductId,
                        DisplayOrder = model.DisplayOrder,
                        Plantform = model.Plantform,
                        PictureId = model.PictureId,
                    };

                    _customProductGroupService.InsertCustomProductGroupItem(customProductGroupItem);
                }
                else
                {
                    var cpgi = _customProductGroupService.GetCustomProductGroupItemById(model.CustomProductGroupItemId);
                    if (cpgi == null)
                        //No record found with the specified id
                        return RedirectToAction("Configure");

                    cpgi.PictureId = model.PictureId;
                    cpgi.ProductId = model.ProductId;
                    cpgi.DisplayOrder = model.DisplayOrder;
                    cpgi.Plantform = model.Plantform;

                    _customProductGroupService.UpdateCustomProductGroupItem(cpgi);
                }

                ViewBag.RefreshPage = true;
                return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditProductPopup", model);
            }

            ViewBag.RefreshPage = false;
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditProductPopup", model);
        }


        public ActionResult EditProductPopup2(int? customProductGroupId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");
            if (!customProductGroupId.HasValue)
            {
                return RedirectToAction("Configure");

            }
            var model = new ProductAddModel();
            ViewBag.CustomProductGroupId = customProductGroupId.Value;
            PrepareProductAddModel(model);
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.EditProductPopup2", model);
        }
        [HttpPost]
        public ActionResult EditProductPopup2(int? customProductGroupId, ProductAddModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Json(new { Success = false, Msg = "没有权限!" });
            if (customProductGroupId.HasValue)
            {
                if (model.SelectedProductIds == null || model.SelectedProductIds.Length == 0)
                {
                    return Json(new { Success = false, Msg = "没有选择商品!" });
                }
                var productList = _productService.GetProductsByIds(model.SelectedProductIds);

                var customProductGroup = _customProductGroupService.GetCustomProductGroupById(customProductGroupId.Value);
                var customProductGropItems = _customProductGroupService.GetCustomProductGroupItems(customProductGroupId.Value, 10000, PlantformType.Default).ToList();
                var customProductGroupItemProductIdList = customProductGropItems.Select(s => s.ProductId);
                foreach (var p in productList)
                {
                    if (customProductGroupItemProductIdList.Contains(p.Id))//更新
                    {
                        var groupItem = customProductGropItems.Find(s => s.ProductId == p.Id);
                        groupItem.Plantform = model.Plantform;
                        _customProductGroupService.UpdateCustomProductGroupItem(groupItem);
                    }
                    else//添加
                    {
                        var customProductGroupItem = new CustomProductGroupItem()
                        {
                            CustomProductGroupId = customProductGroupId.Value,
                            ProductId = p.Id,
                            DisplayOrder = 0,
                            Plantform = model.Plantform,

                        };
                        _customProductGroupService.InsertCustomProductGroupItem(customProductGroupItem);
                    }

                }

                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false, Msg = "主题Id没有值,请求非法!" });
            }

        }


        private void PrepareProductAddModel(ProductAddModel model)
        {
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var c in _categoryService.GetAllCategories(showHidden: true))
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(_categoryService), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = Nop.Core.Domain.Catalog.ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        }

        public ActionResult RelatedProductAddPopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var model = new Nop.Admin.Models.Catalog.ProductModel.AddRelatedProductModel();
            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var c in _categoryService.GetAllCategories(showHidden: true))
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(_categoryService), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = Nop.Core.Domain.Catalog.ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.ProductSelectPopup", model);
        }

        #endregion

        #region Plugin
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            base.Initialize(requestContext);
        }

        [AdminAuthorize]
        public ActionResult Configure(string d, string pcid, string pid)
        {
            var model = new Models.ConfigurationFormModel();
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.Configure", model);
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var model = _cacheManager.Get<Models.PublicInfoModel>(CUSTOMPRODUCTGROUP_PUBLICINFO_KEY, () => PreparePublicInfoModel());
            return View("Nop.Plugin.Widgets.CustomProductGroup.Views.WidgetsCustomProductGroup.PublicInfo", model);
        }

        private Models.PublicInfoModel PreparePublicInfoModel()
        {
            var model = new Models.PublicInfoModel();

            var customProductGroups = this._customProductGroupService.GetAllCustomProductGroups(plantformType: PlantformType.PC, onlyShowEnable: true);
            foreach (var customProductGroup in customProductGroups.OrderBy(n => n.DisplayOrder))
            {
                var customProductGroupModel = new Models.CustomProductGroupModel();
                customProductGroupModel.Id = customProductGroup.Id;
                customProductGroupModel.Title = customProductGroup.Title;
                customProductGroupModel.SubCategoryLinks = customProductGroup.SubCategoryLinks;
                customProductGroupModel.MoreLink = customProductGroup.MoreLink;
                customProductGroupModel.Style = customProductGroup.Style;
                customProductGroupModel.AdLink = customProductGroup.AdLink;

                if (customProductGroup.AdLinkPictureId.HasValue)
                {
                    customProductGroupModel.AdLinkPictureId = customProductGroup.AdLinkPictureId.Value;
                    customProductGroupModel.AdLinkPictureUrl = _pictureService.GetPictureUrl(customProductGroupModel.AdLinkPictureId, showDefaultPicture: false);
                }

                var products = new List<Product>();
                var customProductGroupItems = _customProductGroupService.GetCustomProductGroupItems(customProductGroup.Id, 11, PlantformType.PC);

                customProductGroupModel.ProductIds = (from n in customProductGroupItems
                                                      orderby n.DisplayOrder
                                                      select n.ProductId).ToList();

                //foreach (var customProductGroupItem in customProductGroupItems)
                //{
                //    var product = _productService.GetProductById(customProductGroupItem.ProductId);
                //    if (product != null && product.Published)
                //        products.Add(product);
                //}

                //products = _productService.GetProductsByIds(productIds).ToList();
                //customProductGroupModel.Products = products;

                if (customProductGroupItems.Count > 0)
                {
                    var product = _productService.GetProductById(customProductGroupItems[0].ProductId);
                    if (product != null)
                    {
                        customProductGroupModel.FirstProduct = new ProductModel();
                        customProductGroupModel.FirstProduct.SeName = product.GetSeName();
                        // customProductGroupModel.FirstProduct.IsSoldOut = product.IsSoldOut();
                        if (customProductGroup.FirstProductPictureId.HasValue)
                            customProductGroupModel.FirstProduct.PictureUrl = _pictureService.GetPictureUrl(customProductGroup.FirstProductPictureId.Value);
                    }
                }

                model.CustomProductGroupModels.Add(customProductGroupModel);
            }

            return model;
        }

        #endregion
    }
}