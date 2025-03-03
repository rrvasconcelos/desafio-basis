using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using FluentAssertions;

namespace BookStore.UnitTests.Domain.Entities;

public class AuthorTests
{
    private const string DefaultAuthorName = "Robert C. Martin";
    
    private static Author CreateDefaultAuthor()
    {
        return new Author(DefaultAuthorName);
    }
    
    private static Author CreateAuthor(string name = DefaultAuthorName)
    {
        return new Author(name);
    }
    
    [Fact]
    public void Author_Should_Be_Created_With_Valid_Properties()
    {
        // Act
        var author = CreateDefaultAuthor();

        // Assert
        author.Name.Should().Be(DefaultAuthorName);
        author.Active.Should().BeTrue();
    }
    
    [Fact]
    public void Author_Should_Have_CreatedAt_Set_When_Created()
    {
        // Act
        var author = CreateDefaultAuthor();
            
        // Assert
        author.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        // Arrange
        var author = CreateDefaultAuthor();
    
        // Act
        author.Deactivate();
    
        // Assert
        author.Active.Should().BeFalse();
    }
    
    [Fact]
    public void Activate_AfterDeactivation_ShouldSetActiveToTrue()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        author.Deactivate();
    
        // Act
        author.Activate();
    
        // Assert
        author.Active.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Author_Should_Not_Be_Created_With_Invalid_Name(string invalidName)
    {
        // Act
        Action act = () => CreateAuthor(invalidName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Author name cannot be empty");
    }
    
    [Fact]
    public void Author_Should_Have_Empty_BookAuthors_Collection_When_Created()
    {
        // Arrange & Act
        var author = CreateDefaultAuthor();
            
        // Assert
        author.BookAuthors.Should().NotBeNull();
        author.BookAuthors.Should().BeEmpty();
    }
    
    [Fact]
    public void Update_Should_Change_Author_Name()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        const string newName = "Martin Fowler";

        // Act
        author.UpdateName(newName);

        // Assert
        author.Name.Should().Be(newName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Update_Should_Not_Accept_Invalid_Name(string invalidName)
    {
        // Arrange
        var author = CreateDefaultAuthor();

        // Act
        Action act = () => author.UpdateName(invalidName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Author name cannot be empty");
    }
    
    [Fact]
    public void AddBook_Should_Create_Association_Between_Author_And_Book()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        var book = new Book("Clean Code", "Prentice Hall", 1, "2008");

        // Act
        author.AddBook(book);

        // Assert
        author.BookAuthors.Should().HaveCount(1);
        author.BookAuthors.First().Book.Should().Be(book);
        author.BookAuthors.First().Author.Should().Be(author);
    }
    
    [Fact]
    public void AddBook_Should_Not_Allow_Adding_Same_Book_Twice()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        var book = new Book("Clean Code", "Prentice Hall", 1, "2008");
        
        // Act
        author.AddBook(book);
        Action act = () => author.AddBook(book);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Book is already associated with this author*");
    }
    
    [Fact]
    public void RemoveBook_Should_Remove_Association_Between_Author_And_Book()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        var book = new Book("Clean Code", "Prentice Hall", 1, "2008");
        author.AddBook(book);

        // Act
        author.RemoveBook(book);

        // Assert
        author.BookAuthors.Should().BeEmpty();
    }
    
    [Fact]
    public void RemoveBook_Should_Throw_When_Book_Not_Associated()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        var book = new Book("Clean Code", "Prentice Hall", 1, "2008");

        // Act
        Action act = () => author.RemoveBook(book);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Book is not associated with this author*");
    }
    
    [Fact]
    public void GetBooks_Should_Return_All_Associated_Books()
    {
        // Arrange
        var author = CreateDefaultAuthor();
        var book1 = new Book("Clean Code", "Prentice Hall", 1, "2008",1);
        var book2 = new Book("Clean Architecture", "Prentice Hall", 1, "2017",2);
        
        author.AddBook(book1);
        author.AddBook(book2);

        // Act
        var books = author.GetBooks().ToList();

        // Assert
        books.Should().HaveCount(2);
        books.Should().Contain(book1);
        books.Should().Contain(book2);
    }
}