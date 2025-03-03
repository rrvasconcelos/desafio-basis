namespace BookStore.Domain.Catalog;

public interface IBookRepository
{
    Task AddAsync(Book book);
    Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Book book);
    void Delete(Book book);
}