using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
 

using Nop.Web.Framework.Mvc;
using Nop.Plugin.Widgets.ProductSpecialSale.Services;
using Nop.Plugin.Widgets.ProductSpecialSale.Data;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {

            builder.RegisterType<SpecialSaleStageService>().As<ISpecialSaleStageService>().InstancePerHttpRequest();

            
            //data context
            this.RegisterPluginDataContext<ProductSpecialSaleObjectContext>(builder, "nop_object_context_specialsaleobjectcontext");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<SpecialSaleGroup>>()
                .As<IRepository<SpecialSaleGroup>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_specialsaleobjectcontext"))
                .InstancePerHttpRequest();

            builder.RegisterType<EfRepository<SpecialSaleProduct>>()
               .As<IRepository<SpecialSaleProduct>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_specialsaleobjectcontext"))
               .InstancePerHttpRequest();


            builder.RegisterType<EfRepository<SpecialSaleStage>>()
               .As<IRepository<SpecialSaleStage>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_specialsaleobjectcontext"))
               .InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
