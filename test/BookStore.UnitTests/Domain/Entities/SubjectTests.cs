using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.UnitTests.Domain.Entities;

public class SubjectTests
{
    [Fact]
    public void Constructor_WithValidDescription_ShouldCreateSubject()
    {
        // Arrange
        const string description = "Computer Science";

        // Act
        var subject = new Subject(description);

        // Assert
        Assert.Equal(description, subject.Description);
        Assert.True(subject.Active);
        Assert.True(subject.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.Empty(subject.BookSubjects);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ShouldThrowDomainException(string invalidDescription)
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023", 1);

        // Act
        subject.AddBook(book);

        // Assert
        Assert.Single(subject.BookSubjects);
        Assert.Equal(book.Id, subject.BookSubjects.First().BookId);
    }

    [Fact]
    public void UpdateDescription_WithValidDescription_ShouldUpdateDescription()
    {
        // Arrange
        var subject = new Subject("Science");
        const string newDescription = "Computer Science";
    
        // Act
        subject.UpdateDescription(newDescription);
    
        // Assert
        Assert.Equal(newDescription, subject.Description);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateDescription_WithInvalidDescription_ShouldThrowDomainException(string invalidDescription)
    {
        // Arrange
        var subject = new Subject("Science");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => subject.UpdateDescription(invalidDescription));
        Assert.Equal("Subject description cannot be empty", exception.Message);
    }
    
    [Fact]
    public void Activate_WhenInactive_ShouldActivateSubject()
    {
        // Arrange
        var subject = new Subject("Science");
        subject.Deactivate();
        Assert.False(subject.Active);

        // Act
        subject.Activate();

        // Assert
        Assert.True(subject.Active);
    }
    
    [Fact]
    public void AddBook_WithValidBook_ShouldAddBookToSubject()
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023");

        // Act
        subject.AddBook(book);

        // Assert
        Assert.Single(subject.BookSubjects);
        Assert.Equal(book.Id, subject.BookSubjects.First().BookId);
    }
    
    [Fact]
    public void AddBook_WithDuplicateBook_ShouldThrowDomainException()
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023", 1);
        subject.AddBook(book);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => subject.AddBook(book));
        Assert.Equal("Book is already associated with this subject", exception.Message);
    }
    
    [Fact]
    public void AddBook_WhenSubjectIsInactive_ShouldThrowDomainException()
    {
        // Arrange
        var subject = new Subject("Science");
        subject.Deactivate();
        var book = new Book("Test Book", "Test Publisher", 1, "2023");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => subject.AddBook(book));
        Assert.Equal("Cannot add book to inactive subject", exception.Message);
    }
    
    [Fact]
    public void RemoveBook_WithAssociatedBook_ShouldRemoveBookFromSubject()
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023");
        subject.AddBook(book);
        Assert.Single(subject.BookSubjects);

        // Act
        subject.RemoveBook(book);

        // Assert
        Assert.Empty(subject.BookSubjects);
    }
    
    [Fact]
    public void RemoveBook_WithNonAssociatedBook_ShouldThrowDomainException()
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => subject.RemoveBook(book));
        Assert.Equal("Book is not associated with this subject", exception.Message);
    }

    [Fact]
    public void RemoveBook_WhenSubjectIsInactive_ShouldThrowDomainException()
    {
        // Arrange
        var subject = new Subject("Science");
        var book = new Book("Test Book", "Test Publisher", 1, "2023");
        subject.AddBook(book);
        subject.Deactivate();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => subject.RemoveBook(book));
        Assert.Equal("Cannot remove book from inactive subject", exception.Message);
    }
    
    [Fact]
    public void GetBooks_WithMultipleBooks_ShouldReturnAllBooks()
    {
        // Arrange
        var subject = new Subject("Science");
        var book1 = new Book("Test Book 1", "Test Publisher", 1, "2023",1);
        var book2 = new Book("Test Book 2", "Test Publisher", 2, "2023",2);
            
        subject.AddBook(book1);
        subject.AddBook(book2);

        // Act
        var books = subject.GetBooks().ToList();

        // Assert
        Assert.Equal(2, books.Count);
    }
}