using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale
{
    public class ViewPathHelper
    {
        private readonly static string _Widget_Path_Format = "~/Plugins/Widgets.ProductSpecialSale/Views/WidgetsProductSpecialSale/{0}.cshtml";

        public static string GetViewPath(string viewName)
        {
            return string.Format(_Widget_Path_Format, viewName);
        }
    }
}
