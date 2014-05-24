using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomProductGroup.Data
{
    public class CustomProductGroupMap : EntityTypeConfiguration<CustomProductGroup.Domain.CustomProductGroup>
    {
        public CustomProductGroupMap()
        {
            this.ToTable("CustomProductGroup");
            this.HasKey(c => c.Id);

            this.Property(c => c.Title).IsRequired();
            this.Property(c => c.Style).IsRequired();
            this.Property(c => c.SubCategoryLinks).IsRequired();
            this.Property(c => c.MoreLink).IsRequired();
            this.Property(c => c.IsEnable).IsRequired();
            this.Property(c => c.IsRecommended).IsRequired();
            this.Property(c => c.DisplayOrder).IsRequired();
            this.Property(c => c.PictureId);
            this.Property(c => c.FirstProductPictureId);
            this.Property(c => c.AdLink);
            this.Property(c => c.AdLinkPictureId);
            this.Property(c => c.Plantform).IsRequired();

            this.Ignore(c => c.IsSelected);
        }
    }
}
