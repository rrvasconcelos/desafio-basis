using BookStore.Domain.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Authors;

public sealed class AuthorConfiguration: IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(40);
        builder.Property(a => a.Active).IsRequired();
        builder.Property(a => a.CreatedAt).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();
    }
}