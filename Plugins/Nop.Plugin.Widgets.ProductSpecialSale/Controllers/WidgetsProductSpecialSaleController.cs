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

        private readonly IActivityService _activityService;
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IWorkContext _workContext;
        private readonly IPromptActivityService _promptActivityService;
        private readonly ISettingService _settingService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IPromotionService _promotionService;
        public WidgetsProductSpecialSaleController(IActivityService activityService, IProductService productService,
                                  IPictureService pictureService,
                                  ILocalizationService localizationService, IPriceFormatter priceFormatter,
                                  IDateTimeHelper dateTimeHelper, IWorkContext workContext,
                                  IPromptActivityService promptActivityService, ISettingService settingService,
                                  ICustomerService customerService, IOrderService orderService, IPromotionService promotionService)
        {
            this._activityService = activityService;
            this._productService = productService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._priceFormatter = priceFormatter;
            this._dateTimeHelper = dateTimeHelper;
            this._workContext = workContext;
            this._promptActivityService = promptActivityService;
            this._settingService = settingService;
            this._customerService = customerService;
            this._orderService = orderService;
            _promotionService = promotionService;
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


        /// <summary>
        /// 获取活动平台下产品分组的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int activityId)
        {
            var model = PrepareActivityModelByActivityId(activityId);
            return View("ActivityInfo", model);
        }

        public PromptModel PreparePromptModelByActivityType(int activityType)
        {
            PromptModel prompt = new PromptModel();

            var customer = _workContext.CurrentCustomer;
            if (customer != null)
                prompt.CustomerId = _workContext.CurrentCustomer.Id;
            //TODO:手机，帐号
            //if (!string.IsNullOrEmpty(customer.Mobile))
            //{
            //    prompt.PhoneEnabled = true;
            //    prompt.Phone = customer.Mobile;
            //}
            //if (!string.IsNullOrEmpty(customer.Mobile))
            //{
            //    prompt.PhoneEnabled = true;
            //    prompt.Email = customer.Email;
            //}

            var activity = _activityService.GetActivityByType(activityType);
            var activityProducts =
                activity.Products.Where(
                    n => n.Promotion.EndDateTimeUtc > DateTime.UtcNow &&
                         !n.Product.Deleted && n.Product.Published && !n.Product.DisableBuyButton &&
                         n.IsPcPlatformAvailable)
                        .ToList();

            //今日特价
            DateTime todayStartDateTime = _dateTimeHelper.ConvertToUtcTime(DateTime.Today, DateTimeKind.Local);
            DateTime todayEndDateTime = todayStartDateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime tomorrowStartDateTime = todayStartDateTime.AddDays(1);
            DateTime tomorrowEndDateTime = todayEndDateTime.AddDays(1);
            prompt.ActivityId = activity.Id;
            foreach (var activityProduct in activityProducts)
            {
                if (activityProduct.Promotion.StartDateTimeUtc <= todayEndDateTime &&
                    activityProduct.Promotion.EndDateTimeUtc >= todayStartDateTime)
                {
                    prompt.IsToday = true;
                    prompt.TodayPromptProductList.Add(GetPromptProduct(activityProduct, ActivityProgressStatus.InProgress));
                }
                else if (activityProduct.Promotion.StartDateTimeUtc <= tomorrowEndDateTime &&
                         activityProduct.Promotion.EndDateTimeUtc >= tomorrowStartDateTime)
                {
                    prompt.IsTomorrow = true;
                    prompt.TomorrowPromptProductList.Add(GetPromptProduct(activityProduct, ActivityProgressStatus.UpComing));
                }
                else
                {
                    prompt.IsFuture = true;
                    //var isOutTime = (activityProduct.Promotion.StartDateTimeUtc > DateTime.UtcNow &&
                    //       activityProduct.Promotion.EndDateTimeUtc > DateTime.UtcNow);
                    //if (isOutTime)
                    prompt.FuturePromptProductList.Add(GetPromptProduct(activityProduct, ActivityProgressStatus.UpComing));
                }
            }
            return prompt;
        }

        public PromptProduct GetPromptProduct(ActivityProduct actProduct, ActivityProgressStatus progressStatus)
        {
            if (actProduct == null)
                throw new ArgumentNullException("activityProduct");
            var isStrat = (actProduct.Promotion.StartDateTimeUtc < DateTime.UtcNow &&
                           actProduct.Promotion.EndDateTimeUtc > DateTime.UtcNow);
            var promptProduct = new PromptProduct();
            promptProduct.ProductId = actProduct.ProductId;
            promptProduct.Data = "";

            promptProduct.EndTimeUtc = actProduct.Promotion.EndDateTimeUtc;
            promptProduct.StartTimeUtc = actProduct.Promotion.StartDateTimeUtc;
            promptProduct.IsStrat = isStrat;
            var productPictures = actProduct.Product.ProductPictures.ToList();
            var productPictureUrl = string.Empty;
            if (productPictures.Any())
            {
                productPictureUrl = _pictureService.GetPictureUrl(productPictures[0].PictureId, 0, false, null,
                    PictureType.Entity);
            }
            promptProduct.ProductPictureUrl = productPictureUrl;
            promptProduct.ProductName = actProduct.Product.Name;
            promptProduct.ProductSename = actProduct.Product.GetSeName();
            promptProduct.ProductDesc = actProduct.Product.ShortDescription;
            promptProduct.Price = _priceFormatter.FormatPrice(actProduct.Product.Price);
            promptProduct.OldPrice = _priceFormatter.FormatPrice(actProduct.Product.OldPrice);
            promptProduct.DisplayOrder = actProduct.DisplayOrder;
            //售罄
            //promptProduct.IsSoldOut = actProduct.Product.IsSoldOut();
            var promotionProduct = actProduct.Promotion.Products.FirstOrDefault(n => n.PromotionId == actProduct.Promotion.Id && n.ProductId == actProduct.Product.Id);
            switch (progressStatus)
            {
                case ActivityProgressStatus.UpComing:
                    promptProduct.PcPlatformPrice = _priceFormatter.FormatPrice(promotionProduct != null ? promotionProduct.PcPlatformPrice : actProduct.Product.Price);
                    break;
                case ActivityProgressStatus.InProgress:
                    promptProduct.PcPlatformPrice = _priceFormatter.FormatPrice(actProduct.Product.SpecialPrice == null ? actProduct.Product.Price : actProduct.Product.SpecialPrice.Value);
                    break;
                case ActivityProgressStatus.End:
                    promptProduct.PcPlatformPrice = _priceFormatter.FormatPrice(actProduct.Product.Price);
                    break;
            }

            //折扣
            if (promotionProduct != null && actProduct.Product.Price > 0)
            {
                promptProduct.Discount =
                    Math.Round((promotionProduct.PcPlatformPrice / actProduct.Product.Price) * 10, 1)
                        .ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                promptProduct.DisableOldPrice = true;
            }
            if (promptProduct.PcPlatformPrice == promptProduct.Price)
            {
                promptProduct.DisableOldPrice = true;
            }

            //TODO:客户最大购买数
            //客户购买最大数量
            //if (actProduct.Product.CustomerMaximumQuantity.HasValue)
            //    promptProduct.CustomerMaximumQuantity = actProduct.Product.CustomerMaximumQuantity.Value;

            //已购买人数
            //promptProduct.BuyedCount = _orderService.GetBuyerCount(actProduct.Product.Id, actProduct.Promotion.StartDateTimeUtc, actProduct.Promotion.EndDateTimeUtc);
            return promptProduct;
        }

        public ActivityModel PrepareActivityModelByActivityId(int activityId)
        {
            var activity = _activityService.GetActivityById(activityId);
            var activityProducts =
                activity.Products.Where(
                    n =>
                    !n.Product.Deleted && n.Product.Published && !n.Product.DisableBuyButton && n.IsPcPlatformAvailable)
                        .ToList();
            ActivityModel act = new ActivityModel();
            act.Category = activity.Category;
            act.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(activity.CreatedOnUtc, DateTimeKind.Utc);
            act.Description = activity.Description;
            if (activity.EndTimeUtc.HasValue)
                act.EndTimeUtc = _dateTimeHelper.ConvertToUserTime(activity.EndTimeUtc.Value, DateTimeKind.Utc);
            act.Id = activity.Id;
            act.LastUpdatedTimeUtc = _dateTimeHelper.ConvertToUserTime(activity.LastUpdatedTimeUtc, DateTimeKind.Utc);
            act.Name = activity.Name;
            act.ActivityStyle = activity.Styles.Where(n => n.Platform == (int)ActivityPlatform.Pc).Single();
            if (act.ActivityStyle.PosterPictureId > 0)
                act.PosterPictureUrl = _pictureService.GetPictureUrl(act.ActivityStyle.PosterPictureId, 0, true, null, PictureType.Entity);
            //act.BackgroundPictureUrl = _pictureService.GetPictureUrl((int)act.ActivityStyle.BackgroundPictureId, 0, true, null, PictureType.Entity);
            var groupedProducts = activityProducts.Where(a => a.ActivityProductGroupId != null).OrderBy(a => a.DisplayOrder).ThenByDescending(a => a.Id).ToList();
            var notGroupedProducts = activityProducts.Where(a => a.ActivityProductGroupId == null).OrderBy(a => a.DisplayOrder).ThenByDescending(a => a.Id).ToList();
            //未分组
            if (notGroupedProducts.Count > 0)
            {
                act.HaveNotGroupedProducts = true;
                foreach (var productitem in notGroupedProducts)
                {
                    var model = new ActivityProductModel
                    {
                        ActivityProductGroupId = productitem.ActivityProductGroupId,
                        ProductId = productitem.ProductId,
                        DisplayOrder = productitem.DisplayOrder
                    };
                    var product = _productService.GetProductById(productitem.Product.Id);
                    model.Product = product;
                    model.Price = _priceFormatter.FormatPrice(product.Price);
                    //TODO:售罄
                   // model.IsSoldOut = product.IsSoldOut();
                    if (activity.Category == (int)ActivityCategory.Common ||
                                activity.Category == (int)ActivityCategory.Cycle)
                    {
                        var promotionProduct =
                            productitem.Promotion.Products.FirstOrDefault(
                                n => n.PromotionId == productitem.Promotion.Id && n.ProductId == productitem.Product.Id);
                        var activityProgressStatus = ActivityProgressStatus.UpComing;
                        if (productitem.Promotion.StartDateTimeUtc <= DateTime.UtcNow && productitem.Promotion.EndDateTimeUtc > DateTime.UtcNow)
                        {
                            activityProgressStatus = ActivityProgressStatus.InProgress;
                        }
                        else if (productitem.Promotion.EndDateTimeUtc <= DateTime.UtcNow)
                        {
                            activityProgressStatus = ActivityProgressStatus.End;
                        }
                        switch (activityProgressStatus)
                        {
                            case ActivityProgressStatus.UpComing:
                                model.PcPlatformPrice = _priceFormatter.FormatPrice(promotionProduct != null ? promotionProduct.PcPlatformPrice : productitem.Product.Price);
                                break;
                            case ActivityProgressStatus.InProgress:
                                model.PcPlatformPrice = _priceFormatter.FormatPrice(productitem.Product.SpecialPrice == null ? productitem.Product.Price : productitem.Product.SpecialPrice.Value);
                                break;
                            case ActivityProgressStatus.End:
                                model.PcPlatformPrice = _priceFormatter.FormatPrice(productitem.Product.Price);
                                break;
                        }
                        //model.PcPlatformPrice =
                        //    _priceFormatter.FormatPrice(promotionProduct == null ? 0 : promotionProduct.PcPlatformPrice);
                    }
                    else if (activity.Category == (int)ActivityCategory.NonPromotion)
                    {
                        /*var promotion=_promotionService.GetPromotionByProductId(productitem.ProductId);
                        if (promotion == null)
                        {
                            model.PcPlatformPrice = model.Price;
                        }
                        else
                        {
                            model.PcPlatformPrice = _priceFormatter.FormatPrice(model.Product.SpecialPrice==null?model.Product.Price:model.Product.SpecialPrice.Value);
                        }*/
                        model.PcPlatformPrice = model.Price;
                    }
                    if (productitem.Product.ProductPictures.Count() > 0)
                    {
                        var picture =
                            _productService.GetProductPictureById(productitem.Product.ProductPictures.OrderBy(a => a.DisplayOrder).ToList()[0].Id);
                        if (picture != null)
                        {
                            model.PictureUrl = _pictureService.GetPictureUrl(picture.PictureId, 0, true, null,
                                                                             PictureType.Entity);
                        }
                    }
                    act.ActivityProductList.Add(model);
                }
            }

            //已分组
            if (groupedProducts.Any())
            {
                act.HaveGroupedProducts = true;
                foreach (var groupitem in activity.Groups.OrderBy(a => a.DisplayOrder))
                {
                    var item = new ActivityProductGroupModel();
                    item.Id = groupitem.Id;
                    item.ActivityId = groupitem.ActivityId;
                    item.Name = groupitem.Name;
                    item.BackgroundColor = groupitem.BackgroundColor;
                    item.OrderNumber = groupitem.DisplayOrder;
                    item.Platform = groupitem.Platform;
                    act.ActivityProductGroupList.Add(item);
                }

                foreach (var item in act.ActivityProductGroupList)
                {
                    foreach (var productitem in groupedProducts)
                    {
                        if (item.Id == productitem.ActivityProductGroupId)
                        {
                            var model = new ActivityProductModel();
                            model.ActivityProductGroupId = productitem.ActivityProductGroupId;
                            model.ProductId = productitem.ProductId;
                            model.DisplayOrder = productitem.DisplayOrder;
                            var product = _productService.GetProductById(productitem.Product.Id);
                            model.Product = product;
                            model.Price = _priceFormatter.FormatPrice(product.Price);
                            //TODO:售罄
                            //model.IsSoldOut = product.IsSoldOut();
                            if (activity.Category == (int)ActivityCategory.Common ||
                                activity.Category == (int)ActivityCategory.Cycle)
                            {
                                var promotionProduct =
                                    productitem.Promotion.Products.Where(
                                        n =>
                                            n.PromotionId == productitem.Promotion.Id &&
                                            n.ProductId == productitem.Product.Id)
                                        .FirstOrDefault();
                                var activityProgressStatus = ActivityProgressStatus.UpComing;
                                if (productitem.Promotion.StartDateTimeUtc <= DateTime.UtcNow && productitem.Promotion.EndDateTimeUtc > DateTime.UtcNow)
                                {
                                    activityProgressStatus = ActivityProgressStatus.InProgress;
                                }
                                else if (productitem.Promotion.EndDateTimeUtc <= DateTime.UtcNow)
                                {
                                    activityProgressStatus = ActivityProgressStatus.End;
                                }
                                switch (activityProgressStatus)
                                {
                                    case ActivityProgressStatus.UpComing:
                                        model.PcPlatformPrice = _priceFormatter.FormatPrice(promotionProduct != null ? promotionProduct.PcPlatformPrice : productitem.Product.Price);
                                        break;
                                    case ActivityProgressStatus.InProgress:
                                        model.PcPlatformPrice = _priceFormatter.FormatPrice(productitem.Product.SpecialPrice == null ? productitem.Product.Price : productitem.Product.SpecialPrice.Value);
                                        break;
                                    case ActivityProgressStatus.End:
                                        model.PcPlatformPrice = _priceFormatter.FormatPrice(productitem.Product.Price);
                                        break;
                                }
                                //model.PcPlatformPrice =
                                //    _priceFormatter.FormatPrice(promotionProduct == null
                                //        ? 0
                                //        : promotionProduct.PcPlatformPrice);
                            }
                            else if (activity.Category == (int)ActivityCategory.NonPromotion)
                            {
                                /*var promotion = _promotionService.GetPromotionByProductId(productitem.ProductId);
                                if (promotion == null)
                                {
                                    model.PcPlatformPrice = model.Price;
                                }
                                else
                                {
                                    model.PcPlatformPrice = _priceFormatter.FormatPrice(model.Product.SpecialPrice == null ? model.Product.Price : model.Product.SpecialPrice.Value);
                                }*/
                                model.PcPlatformPrice = model.Price;
                            }
                            if (productitem.Product.ProductPictures.Count() > 0)
                            {
                                var picture =
                                    _productService.GetProductPictureById(
                                        productitem.Product.ProductPictures.OrderBy(a => a.DisplayOrder).ToList()[0].Id);
                                if (picture != null)
                                {
                                    model.PictureUrl = _pictureService.GetPictureUrl(picture.PictureId, 0, true, null,
                                                                                     PictureType.Entity);
                                }
                            }
                            item.ActivityProductList.Add(model);
                        }
                    }
                }
            }
            return act;
        }

        /// <summary>
        /// 限时疯抢
        /// </summary>
        /// <param name="activityType"></param>
        /// <returns></returns>
        public ActionResult BuyLimit(int activityType)
        {
            var model = PreparePromptModelByActivityType(activityType);
            return View("BuyLimit", model);
        }

        public ActionResult AddRemind(RemindModel remind)
        {
            //判读是否存在
            if (remind == null)
                throw new ArgumentNullException("remindModel");
            var result = new JsonResult();
            var activityProduct =
                _activityService.GetActivityProductsByProductId(remind.ProductId, remind.ActivityId, true)
                                .ToList()
                                .FirstOrDefault();
            var customer = _customerService.GetCustomerById(remind.CustomerId);
            if (activityProduct != null && customer != null)
            {
                var promptActivity = new ActivityRemind
                {
                    CustomerId = remind.CustomerId,
                    ActivityId = remind.ActivityId,
                    ProductId = remind.ProductId,
                    StartDateTimeUtc = activityProduct.Promotion.StartDateTimeUtc,
                    EndDateTimeUtc = activityProduct.Promotion.EndDateTimeUtc,
                    Status = (int)ActivityRemindStatus.New,
                    CreatedOnUtc = DateTime.UtcNow
                };
                if (!string.IsNullOrEmpty(remind.Phone))
                {
                    promptActivity.IsNeededSms = true;
                }
                if (!string.IsNullOrEmpty(remind.Email))
                {
                    promptActivity.IsNeededEmail = true;
                }
                if (promptActivity.IsNeededEmail || promptActivity.IsNeededSms)
                {
                    _promptActivityService.AddPromptActivity(promptActivity);
                    result.Data = new { Success = true };
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult IsRegister(RemindModel remind)
        {
            if (remind == null)
                throw new ArgumentNullException("remindModel");
            var customer = _workContext.CurrentCustomer;
            var isRegister = customer.IsRegistered();
            var result = new JsonResult();
            //获取提醒记录
            var activityRemind = _promptActivityService.GetSmsPromptActivity(remind.ProductId, remind.ActivityId,
                                                                             remind.CustomerId);
            //已经提醒
            if (isRegister)
            {
                if (activityRemind != null)
                {
                    result.Data =
                        new
                        {
                            Success = 3,
                            //TODO:手机
                            //Phone = activityRemind.IsNeededSms ? customer.Mobile : "",
                            //Email = activityRemind.IsNeededEmail ? customer.Email : ""
                        };
                }
                else
                {
                    result.Data = new { Success = 2, 
                        //Phone = customer.Mobile, 
                        Email = customer.Email 
                    };
                }
            }
            else
            {
                result.Data = new { Success = 1 };
            }
            return Json(result);
        }


      
    }
}
