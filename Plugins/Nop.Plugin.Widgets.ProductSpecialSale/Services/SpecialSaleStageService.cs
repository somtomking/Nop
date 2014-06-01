using Nop.Core;
using Nop.Core.Data;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Services
{
    public class SpecialSaleStageService : ISpecialSaleStageService
    {
        private readonly IRepository<SpecialSaleGroup> _specialSaleGroupRepository;
        private readonly IRepository<SpecialSaleProduct> _specialSaleProductRepository;
        private readonly IRepository<SpecialSaleStage> _specialSaleStageRepository;
        public SpecialSaleStageService(
            IRepository<SpecialSaleGroup> specialSaleGroupRepository,
            IRepository<SpecialSaleProduct> specialSaleProductRepository,
            IRepository<SpecialSaleStage> specialSaleStageRepository
            )
        {
            _specialSaleGroupRepository=specialSaleGroupRepository;
            _specialSaleProductRepository=specialSaleProductRepository;
            _specialSaleStageRepository=specialSaleStageRepository;
        }
        public IPagedList<SpecialSaleStage> QuerySpecialSaleStage(int pageIndex, int pageSize)
        {
            var query=_specialSaleStageRepository.Table;
            var result=from s in query where s.Deleted==false orderby s.LastUpdateTime select s;
            return new PagedList<SpecialSaleStage>(result,pageIndex,pageSize);
        }
    }
}
