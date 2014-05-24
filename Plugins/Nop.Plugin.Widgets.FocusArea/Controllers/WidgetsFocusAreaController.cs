using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.FocusArea.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.FocusArea.Controllers
{
    public class WidgetsFocusAreaController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;

        public WidgetsFocusAreaController(IWorkContext workContext,
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
            var focusAreaSettings = _settingService.LoadSetting<FocusAreaSettings>(storeScope);
            var model = new ConfigurationModel();
            model.BoxPictureId = focusAreaSettings.BoxPictureId;
            model.BoxLink = focusAreaSettings.BoxLink;
            model.Picture1Id = focusAreaSettings.Picture1Id;
            model.Text1 = focusAreaSettings.Text1;
            model.Link1 = focusAreaSettings.Link1;
            model.Picture2Id = focusAreaSettings.Picture2Id;
            model.Text2 = focusAreaSettings.Text2;
            model.Link2 = focusAreaSettings.Link2;
            model.Picture3Id = focusAreaSettings.Picture3Id;
            model.Text3 = focusAreaSettings.Text3;
            model.Link3 = focusAreaSettings.Link3;
            model.Picture4Id = focusAreaSettings.Picture4Id;
            model.Text4 = focusAreaSettings.Text4;
            model.Link4 = focusAreaSettings.Link4;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.BoxPictureId_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.BoxPictureId, storeScope);
                model.BoxLink_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.BoxLink, storeScope);
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(focusAreaSettings, x => x.Link4, storeScope);
            }

            return View("Nop.Plugin.Widgets.FocusArea.Views.WidgetsFocusArea.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var focusAreaSettings = _settingService.LoadSetting<FocusAreaSettings>(storeScope);
            focusAreaSettings.BoxPictureId = model.BoxPictureId;
            focusAreaSettings.BoxLink = model.BoxLink;
            focusAreaSettings.Picture1Id = model.Picture1Id;
            focusAreaSettings.Text1 = model.Text1;
            focusAreaSettings.Link1 = model.Link1;
            focusAreaSettings.Picture2Id = model.Picture2Id;
            focusAreaSettings.Text2 = model.Text2;
            focusAreaSettings.Link2 = model.Link2;
            focusAreaSettings.Picture3Id = model.Picture3Id;
            focusAreaSettings.Text3 = model.Text3;
            focusAreaSettings.Link3 = model.Link3;
            focusAreaSettings.Picture4Id = model.Picture4Id;
            focusAreaSettings.Text4 = model.Text4;
            focusAreaSettings.Link4 = model.Link4;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.BoxPictureId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.BoxPictureId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.BoxPictureId, storeScope);

            if (model.BoxLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.BoxLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.BoxLink, storeScope);

            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Picture1Id, storeScope);
            
            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Text1, storeScope);
            
            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Link1, storeScope);
            
            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Picture2Id, storeScope);
            
            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Text2, storeScope);
            
            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Link2, storeScope);
            
            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Picture3Id, storeScope);
            
            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Text3, storeScope);
            
            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Link3, storeScope);
            
            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Picture4Id, storeScope);
            
            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(focusAreaSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(focusAreaSettings, x => x.Link4, storeScope);
            
            //now clear settings cache
            _settingService.ClearCache();
            
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var focusAreaSettings = _settingService.LoadSetting<FocusAreaSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();

            model.BoxPicture = _pictureService.GetPictureUrl(focusAreaSettings.BoxPictureId, showDefaultPicture: false);
            model.BoxLink = focusAreaSettings.BoxLink;

            model.Picture1Url = _pictureService.GetPictureUrl(focusAreaSettings.Picture1Id, showDefaultPicture: false);
            model.Text1 = focusAreaSettings.Text1;
            model.Link1 = focusAreaSettings.Link1;

            model.Picture2Url = _pictureService.GetPictureUrl(focusAreaSettings.Picture2Id, showDefaultPicture: false);
            model.Text2 = focusAreaSettings.Text2;
            model.Link2 = focusAreaSettings.Link2;

            model.Picture3Url = _pictureService.GetPictureUrl(focusAreaSettings.Picture3Id, showDefaultPicture: false);
            model.Text3 = focusAreaSettings.Text3;
            model.Link3 = focusAreaSettings.Link3;

            model.Picture4Url = _pictureService.GetPictureUrl(focusAreaSettings.Picture4Id, showDefaultPicture: false);
            model.Text4 = focusAreaSettings.Text4;
            model.Link4 = focusAreaSettings.Link4;

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url))
                //no pictures uploaded
                return Content("");


            return View("Nop.Plugin.Widgets.FocusArea.Views.WidgetsFocusArea.PublicInfo", model);
        }
    }
}