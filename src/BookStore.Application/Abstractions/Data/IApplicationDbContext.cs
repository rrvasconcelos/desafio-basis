using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<Book> Books { get; }
    DbSet<BookAuthor> BookAuthors { get; }
    DbSet<BookSubject> BookSubjects { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<BookPrice> BookPrices { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}