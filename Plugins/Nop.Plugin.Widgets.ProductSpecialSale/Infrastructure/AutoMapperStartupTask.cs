using Nop.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using Nop.Plugin.Widgets.ProductSpecialSale.Models;
namespace Nop.Plugin.Widgets.ProductSpecialSale.Infrastructure
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<SpecialSaleStage, SpecialSaleStageModel>().ForMember(desc => desc.CustomProperties, mo => mo.Ignore());

        }

        public int Order
        {
            get { return 0; }
        }
    }
}
