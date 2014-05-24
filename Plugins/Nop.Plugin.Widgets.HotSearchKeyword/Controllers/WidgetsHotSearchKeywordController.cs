using System;
using System.Web.Mvc;
using System.Web.Routing;  

using Nop.Core;
using Nop.Plugin.Widgets.HotSearchKeyword.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.HotSearchKeyword.Controllers
{
    public class WidgetsHotSearchKeywordController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;

        public WidgetsHotSearchKeywordController(IWorkContext workContext,
            IStoreContext storeContext, IStoreService storeService,
            IPictureService pictureService, ISettingService settingService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var hotSearchKeywordSettings = _settingService.LoadSetting<HotSearchKeywordSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Keywords = hotSearchKeywordSettings.Keywords;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Keywords_OverrideForStore = _settingService.SettingExists(hotSearchKeywordSettings, x => x.Keywords, storeScope);
            }

            return View("Nop.Plugin.Widgets.HotSearchKeyword.Views.HotSearchKeyword.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var hotSearchKeywordSettings = _settingService.LoadSetting<HotSearchKeywordSettings>(storeScope);
            hotSearchKeywordSettings.Keywords = model.Keywords;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Keywords_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(hotSearchKeywordSettings, x => x.Keywords, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(hotSearchKeywordSettings, x => x.Keywords, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var hotSearchKeywordSettings = _settingService.LoadSetting<HotSearchKeywordSettings>(_storeContext.CurrentStore.Id);

            if (string.IsNullOrEmpty(hotSearchKeywordSettings.Keywords))
                return Content("");

            var model = new PublicInfoModel();
            model.KeywordsHtml = BuildKeywordsHtml(hotSearchKeywordSettings.Keywords);

            return View("Nop.Plugin.Widgets.HotSearchKeyword.Views.HotSearchKeyword.PublicInfo", model);
        }

        private string BuildKeywordsHtml(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return "";

            var html = new System.Text.StringBuilder();
            string[] keywordsArr = keywords.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            string linkFormat = "<a target=\"_blank\" href=\"/search?q={0}\">{1}</a>";      
            foreach (var keyword in keywordsArr)
                html.AppendFormat(linkFormat, Url.Encode(keyword),keyword);             

            return html.ToString();
        }
    }
}