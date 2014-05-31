using Nop.Core.Domain.Catalog;
using Nop.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public static class Extensions
    {
        public static Product GetProduct(this SpecialSaleProduct specialSale)
        {
            if (specialSale == null)
            {
                return null;
            }
            var p = EngineContext.Current.Resolve<Nop.Services.Catalog.IProductService>().GetProductById(specialSale.ProdcutId);
            return p;
        }
    }
}
