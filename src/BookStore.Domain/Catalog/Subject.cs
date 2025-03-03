using BookStore.SharedKernel;

namespace BookStore.Domain.Catalog;

public class Subject : Entity
{
    public string Description { get; private set; }
    public bool Active { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public ICollection<BookSubject> BookSubjects { get; private set; } = [];

    private Subject()
    {
    }

    public Subject(string description)
    {
        ValidateDescription(description);
        Description = description;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public Subject(string description, int id)
        : base(id)
    {
        ValidateDescription(description);
        Description = description;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateDescription(string description)
    {
        ValidateDescription(description);
        Description = description;
    }

    public void Activate()
    {
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
    }

    public void AddBook(Book book)
    {
        ValidateActive("Cannot add book to inactive subject");

        if (BookSubjects.Any(bs => bs.BookId == book.Id))
            throw new DomainException("Book is already associated with this subject");

        BookSubjects.Add(new BookSubject(book.Id, Id));
    }

    public void RemoveBook(Book book)
    {
        ValidateActive("Cannot remove book from inactive subject");

        var bookSubject = BookSubjects.FirstOrDefault(bs => bs.BookId == book.Id);
        if (bookSubject == null)
            throw new DomainException("Book is not associated with this subject");

        BookSubjects.Remove(bookSubject);
    }

    public IEnumerable<Book> GetBooks()
    {
        return BookSubjects.Select(bs => bs.Book);
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Subject description cannot be empty");
    }

    private void ValidateActive(string errorMessage)
    {
        if (!Active)
            throw new DomainException(errorMessage);
    }
}