using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    public class ConfigurationFormModel
    {
        public ConfigurationFormModel()
        {     
            this.Message = "";
        }

        public string Message { get; set; }
    }
}
