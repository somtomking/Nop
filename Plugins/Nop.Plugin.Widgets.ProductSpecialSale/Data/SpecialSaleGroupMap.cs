﻿using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Data
{
    public class SpecialSaleGroupMap : EntityTypeConfiguration<SpecialSaleGroup>
    {
        public SpecialSaleGroupMap()
        {
            this.ToTable("SpecialSaleGroup");
            this.HasKey(c => c.Id);

         
        }
    }
}
