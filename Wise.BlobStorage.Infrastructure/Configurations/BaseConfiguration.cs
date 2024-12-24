using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wise.BlobStorage.Domain.Entities;

namespace Wise.BlobStorage.Infrastructure.Configurations
{
    public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnOrder(0)
                .IsRequired();

            builder.Property(x=> x.Guid)
                .IsRequired()
                .HasColumnOrder(1);

            builder.Property(x => x.RecordDate)
               .HasDefaultValueSql("getdate()")
               .IsRequired()
               .HasColumnOrder(100);

            builder.Property(x=> x.UserId)
                .IsRequired(false)
                .HasColumnOrder(101);

            builder.Property(x=> x.ActionUser)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnOrder(102);

            builder.Property(x => x.CreatedDate)
                .IsRequired()
                .HasColumnOrder(103);

            builder.Property(x=> x.ModifiedDate)
                .IsRequired(false)
                .HasColumnOrder(104);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnOrder(105);
        }
    }
}
