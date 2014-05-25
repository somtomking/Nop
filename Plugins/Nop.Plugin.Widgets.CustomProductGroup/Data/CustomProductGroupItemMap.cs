using Nop.Plugin.Widgets.CustomProductGroup.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Data
{
    public class CustomProductGroupItemMap : EntityTypeConfiguration<CustomProductGroupItem>
    {
        public CustomProductGroupItemMap()
        {
            this.ToTable("CustomProductGroupItem");
            this.HasKey(c => c.Id);

            this.Property(c => c.CustomProductGroupId).IsRequired();
            this.Property(c => c.ProductId).IsRequired();
            this.Property(c => c.PictureId);
            this.Property(c => c.DisplayOrder).IsRequired();
            this.Property(c => c.Plantform).IsRequired();

            this.HasRequired(item => item.CustomProductGroup)
            .WithMany(c => c.CustomProductGroupItems)
            .HasForeignKey(item => item.CustomProductGroupId);

            //this.HasRequired(cpg => cpg.Product)
            //    .WithMany()
            //    .HasForeignKey(cpg => cpg.ProductId);
        }
    }
}
