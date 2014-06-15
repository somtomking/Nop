using Nop.Plugin.Widgets.ProductSpecialSale.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Data
{
    public class SpecialSaleStageMap : EntityTypeConfiguration<SpecialSaleStage>
    {
        public SpecialSaleStageMap()
        {
            this.ToTable("SpecialSaleStage");
            this.HasKey(c => c.Id);

            this.HasRequired(c => c.SpecialSaleStageGroup)
                .WithMany(c => c.SpecialSaleStages)
                .HasForeignKey(c => c.SaleStageGroupId);
        }
    }
}
