using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.CustomProductGroup.Controllers;
using Nop.Plugin.Widgets.CustomProductGroup.Data;
using Nop.Plugin.Widgets.CustomProductGroup.Domain;
using G=Nop.Plugin.Widgets.CustomProductGroup.Domain;
using Nop.Plugin.Widgets.CustomProductGroup.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.CustomProductGroup.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
         
            builder.RegisterType<CustomProductGroupService>().As<ICustomProductGroupService>().InstancePerHttpRequest();


            //data context
            this.RegisterPluginDataContext<CustomProductGroupObjectContext>(builder, "nop_object_context_customproduct");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<CustomerCustomProductGroup>>()
                .As<IRepository<CustomerCustomProductGroup>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_customproduct"))
                .InstancePerHttpRequest();

            builder.RegisterType<EfRepository<G.CustomProductGroup>>()
               .As<IRepository<G.CustomProductGroup>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_customproduct"))
               .InstancePerHttpRequest();


            builder.RegisterType<EfRepository<CustomProductGroupItem>>()
               .As<IRepository<CustomProductGroupItem>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_customproduct"))
               .InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
