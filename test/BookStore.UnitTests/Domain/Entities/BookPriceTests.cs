using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;

namespace BookStore.UnitTests.Domain.Entities;

public class BookPriceTests
{
    private readonly Book _book = new("Test Book", "Test Publisher", 1, "2023");
    private const PurchaseType PurchaseType = BookStore.Domain.Publishing.PurchaseType.Online;

    [Fact]
    public void Constructor_ShouldCreateBookPrice_WithValidParameters()
    {
        // Arrange
        const decimal value = 29.99m;
            
        // Act
        var bookPrice = new BookPrice(_book, PurchaseType, value);
            
        // Assert
        Assert.Equal(_book, bookPrice.Book);
        Assert.Equal(PurchaseType, bookPrice.PurchaseType);
        Assert.Equal(value, bookPrice.Value);
        Assert.True(bookPrice.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.Equal(0, bookPrice.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void Constructor_ShouldThrowException_WhenValueIsZeroOrNegative(decimal invalidValue)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new BookPrice(_book, PurchaseType, invalidValue));
            
        Assert.Equal("Book price must be greater than zero", exception.Message);
    }
    
    [Fact]
    public void Constructor_ShouldThrowException_WhenBookIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new BookPrice(null!, PurchaseType, 29.99m));
            
        Assert.Equal("Book cannot be null", exception.Message);
    }
    
    [Fact]
    public void UpdateValue_ShouldUpdatePrice_WhenValueIsValid()
    {
        // Arrange
        var bookPrice = new BookPrice(_book, PurchaseType, 29.99m);
        const decimal newValue = 39.99m;
            
        // Act
        bookPrice.UpdateValue(newValue);
            
        // Assert
        Assert.Equal(newValue, bookPrice.Value);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void UpdateValue_ShouldThrowException_WhenValueIsZeroOrNegative(decimal invalidValue)
    {
        // Arrange
        var bookPrice = new BookPrice(_book, PurchaseType, 29.99m);
            
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            bookPrice.UpdateValue(invalidValue));
            
        Assert.Equal("Book price must be greater than zero", exception.Message);
    }
}