using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Domain.Publishing;

public class BookPrice : Entity
{
    public int BookId { get; private set; }
    public decimal Value { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public PurchaseType PurchaseType { get; private set; }
    
    public Book Book { get; private set; }
    
    private BookPrice() { }
    
    public BookPrice(Book book, PurchaseType purchaseType, decimal value)
    {
        ValidateBook(book);
        ValidatePurchaseType(purchaseType);
        ValidateValue(value);
        
        Book = book;
        PurchaseType = purchaseType;
        BookId = book.Id;
        Value = value;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void AddValue(decimal value)
    {
        ValidateValue(value);
        Value = value;
    }
    
    public void UpdateValue(decimal value)
    {
        ValidateValue(value);
        Value = value;
    }
    
    private static void ValidateValue(decimal value)
    {
        if (value <= 0)
            throw new DomainException("Book price must be greater than zero");
    }
    
    private static void ValidateBook(Book book)
    {
        if (book == null)
            throw new DomainException("Book cannot be null");
    }
    
    private static void ValidatePurchaseType(PurchaseType purchaseType)
    {
        if (purchaseType == null)
            throw new DomainException("Purchase type cannot be null");
    }
}