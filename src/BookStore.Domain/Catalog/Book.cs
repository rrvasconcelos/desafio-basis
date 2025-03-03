using System.Text.RegularExpressions;
using BookStore.Domain.Authors;
using BookStore.Domain.Publishing;
using BookStore.SharedKernel;

namespace BookStore.Domain.Catalog;

public class Book : Entity
{
    public string Title { get; private set; }
    public string Publisher { get; private set; }
    public int Edition { get; private set; }
    public string PublicationYear { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool Active { get; private set; }

    public ICollection<BookAuthor> BookAuthors { get; private set; } = [];
    public ICollection<BookSubject> BookSubjects { get; private set; } = [];
    public ICollection<BookPrice> BookPrices { get; private set; } = [];

    private Book()
    {
    }

    public Book(string title, string publisher, int edition, string publicationYear)
    {
        ValidateBookDetails(title, publisher, edition, publicationYear);

        Title = title;
        Publisher = publisher;
        Edition = edition;
        PublicationYear = publicationYear;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Book(string title, string publisher, int edition, string publicationYear, int bookId)
        : base(bookId)
    {
        ValidateBookDetails(title, publisher, edition, publicationYear);

        Title = title;
        Publisher = publisher;
        Edition = edition;
        PublicationYear = publicationYear;
        Active = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(string title, string publisher, int edition, string publicationYear)
    {
        ValidateBookDetails(title, publisher, edition, publicationYear);

        Title = title;
        Publisher = publisher;
        Edition = edition;
        PublicationYear = publicationYear;
    }

    public void UpdateTitle(string title)
    {
        ValidateTitle(title);
        Title = title;
    }

    public void UpdatePublisher(string publisher)
    {
        ValidatePublisher(publisher);
        Publisher = publisher;
    }

    public void UpdateEdition(int edition)
    {
        ValidateEdition(edition);
        Edition = edition;
    }

    public void UpdatePublicationYear(string publicationYear)
    {
        ValidatePublicationYear(publicationYear);
        PublicationYear = publicationYear;
    }

    public void Activate()
    {
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
    }
    
    public void AddAuthors(List<Author> authors)
    {
        ValidateActive("Cannot add authors to inactive book");

        foreach (var author in authors.Where(author => BookAuthors.All(ba => ba.Author.Id != author.Id)))
        {
            AddAuthor(author);
        }
        
        var authorOlds = BookAuthors.Where(ba => authors.All(a => a.Id != ba.Author.Id)).ToList();
        
        foreach (var author in authorOlds)
        {
            BookAuthors.Remove(author);
        }
    }

    public void AddAuthor(Author author)
    {
        ValidateActive("Cannot add author to inactive book");

        if (BookAuthors.Any(ba => ba.AuthorId == author.Id))
            throw new DomainException("Author is already associated with this book");

        BookAuthors.Add(new BookAuthor(author, this));
    }
    
    public void UpdateAuthors(List<Author> authors)
    {
        ValidateActive("Cannot update authors for inactive book");

        foreach (var author in authors.Where(author => BookAuthors.All(ba => ba.AuthorId != author.Id)))
        {
            AddAuthor(author);
        }
        
        var authorOlds = BookAuthors.Where(ba => authors.All(a => a.Id != ba.AuthorId)).ToList();
        
        foreach (var author in authorOlds)
        {
            BookAuthors.Remove(author);
        }
    }

    public void UpdateAuthor(Author author)
    {
        ValidateActive("Cannot update author for inactive book");

        if (BookAuthors.Any(ba => ba.Author.Id == author.Id))
            return;

        BookAuthors.Add(new BookAuthor(author, this));
    }

    public void RemoveAuthor(Author author)
    {
        ValidateActive("Cannot remove author from inactive book");

        var bookAuthor = BookAuthors.FirstOrDefault(ba => ba.Author.Id == author.Id);
        if (bookAuthor == null)
            throw new DomainException("Author is not associated with this book");

        BookAuthors.Remove(bookAuthor);
    }

    public IEnumerable<Author> GetAuthors()
    {
        return BookAuthors.Select(ba => ba.Author);
    }

    public void AddSubjects(List<Subject> subjects)
    {
        ValidateActive("Cannot add subjects to inactive book");

        foreach (var subject in subjects.Where(subject => BookSubjects.All(bs => bs.SubjectId != subject.Id)))
        {
            AddSubject(subject.Id);
        }
    }

    private void AddSubject(int subjectId)
    {
        ValidateActive("Cannot add subject to inactive book");

        if (BookSubjects.Any(bs => bs.SubjectId == subjectId))
            return;

        BookSubjects.Add(new BookSubject(Id, subjectId));
    }

    public void UpdateSubjects(List<Subject> subjects)
    {
        ValidateActive("Cannot update subjects for inactive book");

        // Certifique-se de que subjects não é null
        if (subjects == null || subjects.Count == 0)
        {
            return; // Ou lance uma exceção se desejar
        }

        foreach (var subject in subjects.Where(subject => BookSubjects.All(bs => bs.SubjectId != subject.Id)))
        {
            AddSubject(subject.Id);
        }

        var ids = subjects.Select(e => e.Id).ToArray();

        var bookSubjects = BookSubjects.Where(bs => !ids.Contains(bs.SubjectId)).ToList();

        foreach (var bookSubject in bookSubjects)
        {
            BookSubjects.Remove(bookSubject);
        }
    }

    public void RemoveSubject(Subject subject)
    {
        ValidateActive("Cannot remove subject from inactive book");

        var bookSubject = BookSubjects.FirstOrDefault(bs => bs.Subject.Id == subject.Id);
        if (bookSubject == null)
            throw new DomainException("Subject is not associated with this book");

        BookSubjects.Remove(bookSubject);
    }

    public IEnumerable<Subject> GetSubjects()
    {
        return BookSubjects.Select(bs => bs.Subject);
    }

    public void AddPrice(PurchaseType purchaseType, decimal value)
    {
        ValidateActive("Cannot add price to inactive book");
        ValidatePrice(value);

        var existingPrice = BookPrices.FirstOrDefault(bp => bp.PurchaseType == purchaseType);

        if (existingPrice != null)
        {
            existingPrice.UpdateValue(value);
        }
        else
        {
            BookPrices.Add(new BookPrice(this, purchaseType, value));
        }
    }
    
    public void UpdatePrices(List<(PurchaseType PurchaseType, decimal Value)> prices)
    {
        ValidateActive("Cannot update prices for inactive book");

        foreach (var (purchaseType, value) in prices)
        {
            UpdatePrice(purchaseType, value);
        }
        
        var pricesOld = BookPrices.Where(bp => prices.All(p => p.PurchaseType != bp.PurchaseType)).ToList();
        
        foreach (var price in pricesOld)
        {
            BookPrices.Remove(price);
        }
    }

    public void UpdatePrice(PurchaseType purchaseType, decimal value)
    {
        ValidateActive("Cannot update price for inactive book");
        ValidatePrice(value);

        var bookPrice = BookPrices.FirstOrDefault(bp => bp.PurchaseType == purchaseType);
        if (bookPrice == null)
        {
            AddPrice(purchaseType, value);

            return;
        }

        bookPrice.UpdateValue(value);
    }

    public void RemovePrice(PurchaseType purchaseType)
    {
        ValidateActive("Cannot remove price from inactive book");

        var bookPrice = BookPrices.FirstOrDefault(bp => bp.PurchaseType == purchaseType);
        if (bookPrice == null)
            throw new DomainException("Price for this purchase type does not exist");

        BookPrices.Remove(bookPrice);
    }

    public decimal GetPrice(PurchaseType purchaseType)
    {
        var bookPrice = BookPrices.FirstOrDefault(bp => bp.PurchaseType == purchaseType);
        if (bookPrice == null)
            throw new DomainException("Price for this purchase type does not exist");

        return bookPrice.Value;
    }

    public IEnumerable<(PurchaseType PurchaseType, decimal Value)> GetPrices()
    {
        return BookPrices.Select(bp => (bp.PurchaseType, bp.Value));
    }

    public void Delete()
    {
        ValidateActive("Cannot delete inactive book");
        Deactivate();
    }

    public bool HasPrice(PurchaseType purchaseType)
    {
        return BookPrices.Any(bp => bp.PurchaseType == purchaseType);
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Book title cannot be empty");
    }

    private static void ValidatePublisher(string publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher))
            throw new DomainException("Book publisher cannot be empty");
    }

    private static void ValidateEdition(int edition)
    {
        if (edition <= 0)
            throw new DomainException("Book edition must be greater than zero");
    }

    private static void ValidatePublicationYear(string publicationYear)
    {
        if (string.IsNullOrWhiteSpace(publicationYear))
            throw new DomainException("Publication year must be a valid 4-digit year");

        if (!Regex.IsMatch(publicationYear, @"^\d{4}$"))
            throw new DomainException("Publication year must be a valid 4-digit year");
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Book price must be greater than zero");
    }

    private void ValidateActive(string errorMessage)
    {
        if (!Active)
            throw new DomainException(errorMessage);
    }

    private static void ValidateBookDetails(string title, string publisher, int edition, string publicationYear)
    {
        ValidateTitle(title);
        ValidatePublisher(publisher);
        ValidateEdition(edition);
        ValidatePublicationYear(publicationYear);
    }
}