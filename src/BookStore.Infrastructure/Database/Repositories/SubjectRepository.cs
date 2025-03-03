using BookStore.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Database.Repositories;

public class SubjectRepository(BookStoreDbContext context) : ISubjectRepository
{
    public void Add(Subject subject)
    {
        context.Subjects.Add(subject);
    }

    public void Update(Subject subject)
    {
        context.Subjects.Update(subject);
    }

    public void Delete(Subject subject)
    {
        context.Subjects.Remove(subject);
    }

    public async Task<Subject?> GetByDescriptionAsync(string title, CancellationToken cancellationToken)
    {
        return await context.Subjects.FirstOrDefaultAsync(e => e.Description == title,
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Subject>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Subjects.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Subject?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Subjects.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> CheckIfDescriptionExistsAsync(string description, int authorId,
        CancellationToken cancellationToken)
    {
        return await context.Subjects.AsNoTracking().AnyAsync(e => e.Description == description && !e.Id.Equals(authorId), cancellationToken);
    }
}