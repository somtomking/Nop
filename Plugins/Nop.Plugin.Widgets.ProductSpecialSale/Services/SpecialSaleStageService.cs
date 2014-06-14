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
        private readonly IRepository<SpecialSaleStage> _specialSaleStageRepository;
        private readonly IRepository<SpecialSaleProduct> _specialSaleProductRepository;
        private readonly IRepository<SpecialSaleStageGroup> _specialSaleStageGroupRepository;
        public SpecialSaleStageService(
            IRepository<SpecialSaleStage> specialSaleStageRepository,
            IRepository<SpecialSaleProduct> specialSaleProductRepository,
            IRepository<SpecialSaleStageGroup> specialSaleStageGroupRepository
            )
        {
            _specialSaleStageRepository=specialSaleStageRepository;
            _specialSaleProductRepository=specialSaleProductRepository;
            _specialSaleStageGroupRepository=specialSaleStageGroupRepository;
        }
        public IPagedList<SpecialSaleStageGroup> QuerySpecialSaleStage(int pageIndex, int pageSize)
        {
            var query=_specialSaleStageGroupRepository.Table;
            var result=from s in query where s.Deleted==false orderby s.LastUpdateTime select s;
            return new PagedList<SpecialSaleStageGroup>(result,pageIndex,pageSize);
        }


        public void CreateSpecialSaleStageGroup(SpecialSaleStageGroup data)
        {
            data.CreateTime = DateTime.Now;
            data.LastUpdateTime = DateTime.Now;
            _specialSaleStageGroupRepository.Insert(data);
        }

        public SpecialSaleStageGroup GetSpecialSaleStageGroupById(int id)
        {
          return  _specialSaleStageGroupRepository.GetById(id);
        }

        public void UpdateSpecialSaleStageGroup(SpecialSaleStageGroup data)
        {
            data.LastUpdateTime = DateTime.Now;
              _specialSaleStageGroupRepository.Update(data);
        }
    }
}
