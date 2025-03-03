using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Catalog;

public sealed class BookSubjectConfiguration: IEntityTypeConfiguration<BookSubject>
{
    public void Configure(EntityTypeBuilder<BookSubject> builder)
    {
        builder.HasKey(x => new { x.BookId, x.SubjectId });

        builder.HasOne(x => x.Book)
            .WithMany(x => x.BookSubjects)
            .HasForeignKey(x => x.BookId);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.BookSubjects)
            .HasForeignKey(x => x.SubjectId);
    }
}