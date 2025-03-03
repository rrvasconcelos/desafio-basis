namespace BookStore.Domain.Authors;

public interface IAuthorRepository
{
    Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> CheckIfAuthorNameExistsAsync(string name, int authorId, CancellationToken cancellationToken);
    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Author>> GetAllAsync(CancellationToken cancellationToken);
    void Add(Author author);
    void Update(Author author);
    void Delete(Author author);
}