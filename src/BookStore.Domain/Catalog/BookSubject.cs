namespace BookStore.Domain.Catalog;

public class BookSubject
{
    public int BookId { get; private set; }
    public int SubjectId { get; private set; }

    public Book Book { get; private set; } = null!;
    public Subject Subject { get; private set; } = null!;
    
    private BookSubject() { }

    public BookSubject(int bookId, int subjectId)
    {
        BookId = bookId;
        SubjectId = subjectId;
    }
}