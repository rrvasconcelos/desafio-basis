using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using FluentAssertions;

namespace BookStore.UnitTests.Domain.Entities;

public class BookTests
{
    private const string DefaultTitle = "Clean Code";
    private const string DefaultPublisher = "Prentice Hall";
    private const int DefaultEdition = 1;
    private const string DefaultYear = "2008";

    private static Book CreateDefaultBook()
    {
        return new Book(DefaultTitle, DefaultPublisher, DefaultEdition, DefaultYear);
    }

    private static Book CreateBook(
        string title = DefaultTitle,
        string publisher = DefaultPublisher,
        int edition = DefaultEdition,
        string publicationYear = DefaultYear)
    {
        return new Book(title, publisher, edition, publicationYear);
    }

    [Fact]
    public void Book_Should_Be_Created_With_Valid_Properties()
    {
        // Act
        var book = CreateDefaultBook();

        // Assert
        book.Title.Should().Be(DefaultTitle);
        book.Publisher.Should().Be(DefaultPublisher);
        book.Edition.Should().Be(DefaultEdition);
        book.PublicationYear.Should().Be(DefaultYear);
        book.Active.Should().BeTrue();
    }

    [Fact]
    public void Book_Should_Have_CreatedAt_Set_When_Created()
    {
        // Act
        var book = CreateDefaultBook();

        // Assert
        book.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Publisher", 1, "2008")]
    [InlineData(null, "Publisher", 1, "2008")]
    [InlineData("   ", "Publisher", 1, "2008")]
    public void Book_Should_Not_Be_Created_With_Invalid_Title(string invalidTitle, string publisher, int edition,
        string year)
    {
        // Act
        var act = () => CreateBook(invalidTitle, publisher, edition, year);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Book title cannot be empty");
    }

    [Theory]
    [InlineData("Title", "", 1, "2008")]
    [InlineData("Title", null, 1, "2008")]
    [InlineData("Title", "   ", 1, "2008")]
    public void Book_Should_Not_Be_Created_With_Invalid_Publisher(string title, string invalidPublisher, int edition, string year)
    {
        // Act
        Action act = () => CreateBook(title, invalidPublisher, edition, year);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Book publisher cannot be empty");
    }

    [Theory]
    [InlineData("Title", "Publisher", 0, "2008")]
    [InlineData("Title", "Publisher", -1, "2008")]
    public void Book_Should_Not_Be_Created_With_Invalid_Edition(string title, string publisher, int invalidEdition, string year)
    {
        // Act
        Action act = () => CreateBook(title, publisher, invalidEdition, year);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Book edition must be greater than zero");
    }

    [Theory]
    [InlineData("Title", "Publisher", 1, "")]
    [InlineData("Title", "Publisher", 1, null)]
    [InlineData("Title", "Publisher", 1, "   ")]
    [InlineData("Title", "Publisher", 1, "12")]
    [InlineData("Title", "Publisher", 1, "12345")]
    [InlineData("Title", "Publisher", 1, "abcd")]
    public void Book_Should_Not_Be_Created_With_Invalid_PublicationYear(string title, string publisher, int edition, string invalidYear)
    {
        // Act
        Action act = () => CreateBook(title, publisher, edition, invalidYear);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Publication year must be a valid 4-digit year");
    }

    [Fact]
    public void AddPrice_Should_Create_Association_Between_Book_And_PurchaseType_With_Price()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 
        decimal price = 29.99m;

        // Act
        book.AddPrice(purchaseType, price);

        // Assert
        book.BookPrices.Should().HaveCount(1);
        var bookPrice = book.BookPrices.First();
        bookPrice.Book.Should().Be(book);
        bookPrice.PurchaseType.Should().Be(purchaseType);
        bookPrice.Value.Should().Be(price);
    }
    
    [Fact]
    public void AddPrice_Should_Update_Price_When_PurchaseType_Already_Exists()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 
        var initialPrice = 29.99m;
        var updatedPrice = 34.99m;
    
        book.AddPrice(purchaseType, initialPrice);

        // Act
        book.AddPrice(purchaseType, updatedPrice);

        // Assert
        book.BookPrices.Should().HaveCount(1);
        book.BookPrices.First().Value.Should().Be(updatedPrice);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-29.99)]
    public void AddPrice_Should_Not_Accept_Invalid_Price(decimal invalidPrice)
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 

        // Act
        Action act = () => book.AddPrice(purchaseType, invalidPrice);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Book price must be greater than zero");
    }
    
    [Fact]
    public void RemovePrice_Should_Remove_Price_For_PurchaseType()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 
        decimal price = 29.99m;
    
        book.AddPrice(purchaseType, price);

        // Act
        book.RemovePrice(purchaseType);

        // Assert
        book.BookPrices.Should().BeEmpty();
    }
    
    [Fact]
    public void RemovePrice_Should_Throw_When_PurchaseType_Not_Associated()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 

        // Act
        Action act = () => book.RemovePrice(purchaseType);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Price for this purchase type does not exist*");
    }
    
    [Fact]
    public void GetPrice_Should_Return_Price_For_PurchaseType()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 
        decimal price = 29.99m;
    
        book.AddPrice(purchaseType, price);

        // Act
        var result = book.GetPrice(purchaseType);

        // Assert
        result.Should().Be(price);
    }
    
    [Fact]
    public void GetPrice_Should_Throw_When_PurchaseType_Not_Associated()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 

        // Act
        Action act = () => book.GetPrice(purchaseType);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Price for this purchase type does not exist*");
    }
    
    [Fact]
    public void GetPrices_Should_Return_All_Prices()
    {
        // Arrange
        var book = CreateDefaultBook();
        var onlinePurchaseType = PurchaseType.Online;  
        var storePurchaseType = PurchaseType.PhysicalStore; 
    
        book.AddPrice(onlinePurchaseType, 29.99m);
        book.AddPrice(storePurchaseType, 34.99m);

        // Act
        var prices = book.GetPrices().ToList();

        // Assert
        prices.Should().HaveCount(2);
        prices.Should().Contain(p => p.PurchaseType == onlinePurchaseType && p.Value == 29.99m);
        prices.Should().Contain(p => p.PurchaseType == storePurchaseType && p.Value == 34.99m);
    }
    
    [Fact]
    public void HasPrice_Should_Return_True_When_Price_Exists_For_PurchaseType()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 
    
        book.AddPrice(purchaseType, 29.99m);

        // Act
        var result = book.HasPrice(purchaseType);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void HasPrice_Should_Return_False_When_Price_Does_Not_Exist_For_PurchaseType()
    {
        // Arrange
        var book = CreateDefaultBook();
        var purchaseType = PurchaseType.Online; 

        // Act
        var result = book.HasPrice(purchaseType);

        // Assert
        result.Should().BeFalse();
    }
}
