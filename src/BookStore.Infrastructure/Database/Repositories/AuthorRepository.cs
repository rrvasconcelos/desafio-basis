using BookStore.Domain.Authors;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Database.Repositories;

public sealed class AuthorRepository(BookStoreDbContext context) : IAuthorRepository
{
    public async Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Authors.FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }

    public async Task<bool> CheckIfAuthorNameExistsAsync(string name, int authorId, CancellationToken cancellationToken)
    {
        return await context.Authors.AsNoTracking()
            .AnyAsync(a => a.Name == name && a.Id != authorId, cancellationToken);
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context
            .Authors
            .FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public async Task<List<Author>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Authors.AsNoTracking().ToListAsync(cancellationToken);
    }

    public void Add(Author author)
    {
        context.Authors.Add(author);
    }

    public void Delete(Author author)
    {
        context.Authors.Remove(author);
    }

    public void Update(Author author)
    {
        context.Authors.Update(author);
    }
}