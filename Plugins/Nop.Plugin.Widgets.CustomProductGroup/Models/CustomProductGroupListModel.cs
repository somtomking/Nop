using System;
using Nop.Core.Domain.Common;
using Nop.Plugin.Widgets.CustomProductGroup.Domain;

namespace Nop.Plugin.Widgets.CustomProductGroup.Models
{
    public class CustomProductGroupListModel
    {
        public CustomProductGroupListModel()
        {
            Plantform = PlantformType.Default;
        }

        public bool OnlyShowEnable { get; set; }
        public PlantformType Plantform { get; set; }
    }
}
