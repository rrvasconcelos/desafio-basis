using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Database.Repositories;

public sealed class BookRepository(BookStoreDbContext context) : IBookRepository
{
    public async Task AddAsync(Book book)
    {
        await context.Books.AddAsync(book);
    }

    public async Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await context.Books.FirstOrDefaultAsync(e=> e.Title == title, cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context
            .Books
            .Include(c=> c.BookAuthors)
            .Include(c=> c.BookSubjects)
            .Include(c=> c.BookPrices)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context
            .Books
            .Include(e=> e.BookAuthors)
            .Include(e=> e.BookSubjects)
            .Include(e=> e.BookPrices)
            .AsNoTracking().ToListAsync(cancellationToken);
    }

    public void Update(Book book)
    {
        context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        context.Books.Remove(book);
    }
}