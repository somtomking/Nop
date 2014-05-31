using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public partial interface IPromptActivityService
    {
        void AddPromptActivity(ActivityRemind promptActivity);

        ActivityRemind GetSmsPromptActivity(int productId, int activityId, int customerId);
        ActivityRemind GetEmailPromptActivity(int productId, int activityId, int customerId);
        void UpdatePromptActivity(ActivityRemind promptActivity);
        IPagedList<ActivityRemind> SearchUnProcessedActivityRemind(int aheadMinutes, int pageIndex, int pageSize);
        IList<Core.Domain.Catalog.Product> GetRemindProductsByDate(DateTime dateTime, int? maxNumber);
    }
}
