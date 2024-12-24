using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wise.BlobStorage.Domain.Entities;

namespace Wise.BlobStorage.Infrastructure.Configurations
{
    public class BlobConfiguration : BaseConfiguration<Blob>
    {
        public override void Configure(EntityTypeBuilder<Blob> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Blobs");


            builder.Property(x => x.ContainerName)
                 .IsRequired(false)
                 .HasMaxLength(150)
                 .HasColumnOrder(2);

            builder.Property(x => x.BlobName)
                .IsRequired(false)
                .HasMaxLength(150)
                .HasColumnOrder(3);
         
            builder.Property(x => x.Data)
                .IsRequired(false)
                .HasColumnOrder(4);
        }
    }
}
