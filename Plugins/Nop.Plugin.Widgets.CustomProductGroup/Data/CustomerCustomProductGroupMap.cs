using Nop.Plugin.Widgets.CustomProductGroup.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Data
{
    public class CustomerCustomProductGroupMap : EntityTypeConfiguration<CustomerCustomProductGroup>
    {
        public CustomerCustomProductGroupMap()
        {
            this.ToTable("Customer_CustomProductGroup");
            this.HasKey(cc => cc.Id);
            this.Property(cc => cc.CustomerId).IsRequired();
            this.Property(cc => cc.CustomProductGroupIds).HasMaxLength(200);

            //this.HasRequired(cc => cc.Customer)
            //                .WithMany()
            //                .HasForeignKey(cc => cc.CustomerId)
            //                .WillCascadeOnDelete(false);
        }
    }
}
