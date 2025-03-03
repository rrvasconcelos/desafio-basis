using BookStore.Domain.Publishing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.BookPrices;

public sealed class BookPriceConfiguration: IEntityTypeConfiguration<BookPrice>
{
    public void Configure(EntityTypeBuilder<BookPrice> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(b=> b.BookId).IsRequired();
        builder.Property(b=> b.Value).IsRequired();
        builder.Property(b=> b.CreatedAt).IsRequired();
        builder.Property(b=> b.PurchaseType).IsRequired();
    }
}