using AutoMapper;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Plugin.Widgets.ProductSpecialSale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale
{
    public static class MappingExtensions
    {
        public static SpecialSaleStageGroupModel ToModel(this SpecialSaleStageGroup entity)
        {
            return Mapper.Map<SpecialSaleStageGroup, SpecialSaleStageGroupModel>(entity);
        }

        public static SpecialSaleStageGroup ToEntity(this SpecialSaleStageGroupModel model)
        {
            return Mapper.Map<SpecialSaleStageGroupModel, SpecialSaleStageGroup>(model);
        }

        public static SpecialSaleStageModel ToModel(this SpecialSaleStage entity)
        {
            return Mapper.Map<SpecialSaleStage, SpecialSaleStageModel>(entity);
        }

        public static SpecialSaleStage ToEntity(this SpecialSaleStageModel model)
        {
            return Mapper.Map<SpecialSaleStageModel, SpecialSaleStage>(model);
        }
    }
}
