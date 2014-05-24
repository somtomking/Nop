using System;
using System.Web.Mvc;
using System.Linq;
using Nop.Core;
using Nop.Plugin.Widgets.PrimaryCategory.Models;
using Nop.Plugin.Widgets.PrimaryCategory.Core;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Services.Catalog;
using Nop.Core.Domain.Media;
using Nop.Services.Localization;
using Nop.Services.Tax;
using Nop.Services.Directory;
using Nop.Services.Seo;

namespace Nop.Plugin.Widgets.PrimaryCategory.Controllers
{
    public class WidgetsPrimaryCategoryController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly MediaSettings _mediaSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ITaxService _taxService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;

        public WidgetsPrimaryCategoryController(IWorkContext workContext,
            IStoreContext storeContext, IStoreService storeService,
            IPictureService pictureService, ISettingService settingService,
            IProductService productService, MediaSettings mediaSettings,
            ILocalizationService localizationService, ITaxService taxService,
            IPriceCalculationService priceCalculationService, ICurrencyService currencyService,
            IPriceFormatter priceFormatter)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._productService = productService;
            this._mediaSettings = mediaSettings;
            this._localizationService = localizationService;
            this._taxService = taxService;
            this._priceCalculationService = priceCalculationService;
            this._currencyService = currencyService;
            this._priceFormatter = priceFormatter;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            
            var primaryCategorySettings = _settingService.LoadSetting<PrimaryCategorySettings>(storeScope);
            var model = new ConfigurationModel();
            for (var i = 1; i <= 9; i++)
            {
                ReflectionHelper.SetValue(model, "Order" + i.ToString(), ReflectionHelper.GetValue(primaryCategorySettings, "Order" + i.ToString()));
                ReflectionHelper.SetValue(model, "Picture" + i.ToString() + "Id", ReflectionHelper.GetValue(primaryCategorySettings, "Picture" + i.ToString() + "Id"));
                ReflectionHelper.SetValue(model, "Link" + i.ToString(), ReflectionHelper.GetValue(primaryCategorySettings, "Link" + i.ToString()));
                
            }
            model.ProductIdsToKill = primaryCategorySettings.ProductIdsToKill;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                for (var i = 1; i <= 9; i++)
                {
                    
                    ReflectionHelper.SetValue(model, "Order" + i.ToString() + "_OverrideForStore", _settingService.SettingExists(primaryCategorySettings, "Order" + i.ToString(), storeScope));
                    ReflectionHelper.SetValue(model, "Picture" + i.ToString() + "Id_OverrideForStore", _settingService.SettingExists(primaryCategorySettings, "Picture" + i.ToString() + "Id", storeScope));
                    ReflectionHelper.SetValue(model, "Link" + i.ToString() + "_OverrideForStore", _settingService.SettingExists(primaryCategorySettings, "Link" + i.ToString(), storeScope));
                }
                model.ProductIdsToKill_OverrideForStore = _settingService.SettingExists(primaryCategorySettings, x => x.ProductIdsToKill, storeScope);
                
            }

            return View("Nop.Plugin.Widgets.PrimaryCategory.Views.WidgetsPrimaryCategory.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var primaryCategorySettings = _settingService.LoadSetting<PrimaryCategorySettings>(storeScope);
            for (var i = 1; i <= 9; i++)
            {
                
                ReflectionHelper.SetValue(primaryCategorySettings, "Order" + i.ToString(), ReflectionHelper.GetValue(model, "Order" + i.ToString()));
                ReflectionHelper.SetValue(primaryCategorySettings, "Picture" + i.ToString() + "Id", ReflectionHelper.GetValue(model, "Picture" + i.ToString() + "Id"));
                ReflectionHelper.SetValue(primaryCategorySettings, "Link" + i.ToString(), ReflectionHelper.GetValue(model, "Link" + i.ToString()));
            }
            primaryCategorySettings.ProductIdsToKill = model.ProductIdsToKill;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            for (var i = 1; i <= 9; i++)
            {
                var order_OverrideForStore = ReflectionHelper.GetValue(model, "Order" + i.ToString() + "_OverrideForStore");
                if (order_OverrideForStore is bool)
                {
                    if ((bool)order_OverrideForStore || storeScope == 0)
                        _settingService.SaveSetting(primaryCategorySettings, "Order" + i.ToString(), storeScope, false);
                    else if (storeScope > 0)
                        _settingService.DeleteSetting(primaryCategorySettings, "Order" + i.ToString(), storeScope);
                }

                var pictureId_OverrideForStore = ReflectionHelper.GetValue(model, "Picture" + i.ToString() + "Id_OverrideForStore");
                if (pictureId_OverrideForStore is bool)
                {
                    if ((bool)pictureId_OverrideForStore || storeScope == 0)
                        _settingService.SaveSetting(primaryCategorySettings, "Picture" + i.ToString() + "Id", storeScope, false);
                    else if (storeScope > 0)
                        _settingService.DeleteSetting(primaryCategorySettings, "Picture" + i.ToString() + "Id", storeScope);
                }

                var link_OverrideForStore = ReflectionHelper.GetValue(model, "Link" + i.ToString() + "_OverrideForStore");
                if (link_OverrideForStore is bool)
                {
                    if ((bool)link_OverrideForStore || storeScope == 0)
                        _settingService.SaveSetting(primaryCategorySettings, "Link" + i.ToString(), storeScope, false);
                    else if (storeScope > 0)
                        _settingService.DeleteSetting(primaryCategorySettings, "Link" + i.ToString(), storeScope);
                }
            }

            if (model.ProductIdsToKill_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(primaryCategorySettings, x => x.ProductIdsToKill, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(primaryCategorySettings, x => x.ProductIdsToKill, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var primaryCategorySettings = _settingService.LoadSetting<PrimaryCategorySettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();

            for (var i = 1; i <= 9; i++)
            {
                var order = ReflectionHelper.GetValue(primaryCategorySettings, "Order" + i.ToString());
                var pictureId = ReflectionHelper.GetValue(primaryCategorySettings, "Picture" + i.ToString() + "Id");
                var link = ReflectionHelper.GetValue(primaryCategorySettings, "Link" + i.ToString());
                if (order == null || string.IsNullOrEmpty(order.ToString()))
                    continue;
                var setting = new PublicInfoModel.Setting
                {
                    Order = (order == null) ? "0" : order.ToString(),
                    PictureUrl = (pictureId is int) ? _pictureService.GetPictureUrl(Convert.ToInt32(pictureId), showDefaultPicture: false) : "",
                    Link = (link == null) ? "" : link.ToString()
                };
                model.listSettings.Add(setting);
            }
            model.listSettings = model.listSettings.OrderBy(n => n.Order).ToList();

            if (primaryCategorySettings.ProductIdsToKill.Any())
            {
                var productIds = primaryCategorySettings.ProductIdsToKill.Split(',');
                foreach (var productId in productIds)
                {
                    int id;
                    if (int.TryParse(productId, out id))
                    {
                        var product = _productService.GetProductById(id);
                        if (product != null)
                        {
                            var productModel = new PublicInfoModel.ProductModel();
                            var isAssociatedProduct = false;
                            var pictures = _pictureService.GetPicturesByProductId(product.Id);
                            //default picture
                            var defaultPictureSize = isAssociatedProduct ?
                                _mediaSettings.AssociatedProductPictureSize :
                                _mediaSettings.ProductDetailsPictureSize;
                            productModel.Name = product.GetLocalized(x => x.Name);
                            productModel.SeName = product.GetSeName();
                            productModel.ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), defaultPictureSize, !isAssociatedProduct);
                            productModel.Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), product.Name);
                            productModel.AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), product.Name);

                            decimal taxRate = decimal.Zero;
                            decimal oldPriceBase = _taxService.GetProductPrice(product, product.OldPrice, out taxRate);
                            decimal finalPriceWithoutDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, false), out taxRate);
                            decimal finalPriceWithDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, true), out taxRate);

                            decimal oldPrice = _currencyService.ConvertFromPrimaryStoreCurrency(oldPriceBase, _workContext.WorkingCurrency);
                            decimal finalPriceWithoutDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithoutDiscountBase, _workContext.WorkingCurrency);
                            decimal finalPriceWithDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithDiscountBase, _workContext.WorkingCurrency);

                            if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
                                productModel.OldPrice = _priceFormatter.FormatPrice(oldPrice);

                            productModel.Price = _priceFormatter.FormatPrice(finalPriceWithoutDiscount);
                            model.Products.Add(productModel);
                        }
                    }
                }
            }

            //var model = new PublicInfoModel();
            //model.Picture1Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture1Id, showDefaultPicture: false);
            //model.Order1 = primaryCategorySettings.Order1;
            //model.Link1 = primaryCategorySettings.Link1;

            //model.Picture2Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture2Id, showDefaultPicture: false);
            //model.Order2 = primaryCategorySettings.Order2;
            //model.Link2 = primaryCategorySettings.Link2;

            //model.Picture3Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture3Id, showDefaultPicture: false);
            //model.Order3 = primaryCategorySettings.Order3;
            //model.Link3 = primaryCategorySettings.Link3;

            //model.Picture4Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture4Id, showDefaultPicture: false);
            //model.Order4 = primaryCategorySettings.Order4;
            //model.Link4 = primaryCategorySettings.Link4;

            //model.Picture5Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture5Id, showDefaultPicture: false);
            //model.Order5 = primaryCategorySettings.Order5;
            //model.Link5 = primaryCategorySettings.Link5;

            //model.Picture6Url = _pictureService.GetPictureUrl(primaryCategorySettings.Picture6Id, showDefaultPicture: false);
            //model.Order6 = primaryCategorySettings.Order6;
            //model.Link6 = primaryCategorySettings.Link6;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url) && string.IsNullOrEmpty(model.Picture6Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.PrimaryCategory.Views.WidgetsPrimaryCategory.PublicInfo", model);
        }
    }
}