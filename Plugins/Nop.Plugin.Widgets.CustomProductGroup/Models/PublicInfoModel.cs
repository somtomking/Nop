using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public PublicInfoModel()
        {
            this.CustomProductGroupModels = new List<CustomProductGroupModel>();
        }

        public List<CustomProductGroupModel> CustomProductGroupModels { get; set; }
    }
}