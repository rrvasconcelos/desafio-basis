namespace BookStore.Domain.Catalog;

public interface ISubjectRepository
{
    Task<Subject?> GetByDescriptionAsync(string title, CancellationToken cancellationToken);
    void Add(Subject subject);
    void Update(Subject subject);
    void Delete(Subject subject);
    Task<IEnumerable<Subject>> GetAllAsync(CancellationToken cancellationToken);
    Task<Subject?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> CheckIfDescriptionExistsAsync(string description, int authorId, CancellationToken cancellationToken);
}