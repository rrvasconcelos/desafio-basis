using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Catalog;

public sealed class SubjectConfiguration: IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(20);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Active).IsRequired();
        
        builder.HasIndex(e=> e.Description).IsUnique();
    }
}