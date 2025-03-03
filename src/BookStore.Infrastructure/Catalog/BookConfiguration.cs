using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Catalog;

public sealed class BookConfiguration: IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Publisher).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Edition).IsRequired();
        builder.Property(x => x.PublicationYear).IsRequired().HasMaxLength(4);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Active).IsRequired();
        
        builder.HasMany(e=> e.BookPrices)
            .WithOne(e => e.Book)
            .HasForeignKey(e=> e.BookId);

        builder.HasIndex(e => e.Title).IsUnique();
    }
}