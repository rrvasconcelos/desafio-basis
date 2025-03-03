using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Domain.Authors;

public class Author: Entity
{
    public string Name { get; private set; }
    public bool Active { get; private set; }
    public DateTimeOffset  CreatedAt { get; private set; }
    public ICollection<BookAuthor> BookAuthors { get; private set; } = [];
    
    private Author() { }
    
    public Author(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Author name cannot be empty");
        
        Name = name;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public Author(int id, string name) 
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Author name cannot be empty");
        
        Name = name;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void Deactivate() => Active = false;
    
    public void Activate() => Active = true;
    
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Author name cannot be empty");
        
        Name = name;
    }
    
    public void AddBook(Book book)
    {
        if (BookAuthors.Any(ba => ba.BookId == book.Id))
            throw new DomainException("Book is already associated with this author");
        
        BookAuthors.Add(new BookAuthor(this, book));
    }
    
    public void RemoveBook(Book book)
    {
        var bookAuthor = BookAuthors.FirstOrDefault(ba => ba.BookId == book.Id);
        if (bookAuthor == null)
            throw new DomainException("Book is not associated with this author");
        
        BookAuthors.Remove(bookAuthor);
    }
    
    public IEnumerable<Book> GetBooks() => BookAuthors.Select(ba => ba.Book);
}