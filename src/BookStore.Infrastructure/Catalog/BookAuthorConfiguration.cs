using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Catalog;

public sealed class BookAuthorConfiguration: IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.HasKey(x => new { x.BookId, x.AuthorId });
        
        builder
            .HasOne(ab => ab.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ab => ab.AuthorId);

        builder
            .HasOne(ab => ab.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ab => ab.BookId);
    }
}