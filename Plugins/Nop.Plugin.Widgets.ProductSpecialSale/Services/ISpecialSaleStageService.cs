using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Core;
namespace Nop.Plugin.Widgets.ProductSpecialSale.Services
{
    public interface ISpecialSaleStageService
    {
        IPagedList<SpecialSaleStageGroup> QuerySpecialSaleStage(int pageIndex, int pageSize);

        void CreateSpecialSaleStageGroup(SpecialSaleStageGroup data);

        SpecialSaleStageGroup GetSpecialSaleStageGroupById(int p);

        void UpdateSpecialSaleStageGroup(SpecialSaleStageGroup data);
    }
}
