using BookStore.Domain.Authors;

namespace BookStore.Domain.Catalog;

public class BookAuthor
{
    public BookAuthor(Author author, Book book)
    {
        Author = author;
        Book = book;
        
        AuthorId = author.Id;
        BookId = book.Id;
    }
    
    private BookAuthor() { }
    
    public int BookId { get; private set; }
    public Book Book { get; private set; }
    
    public int AuthorId { get; private set; }
    public Author Author { get; private set; }
}