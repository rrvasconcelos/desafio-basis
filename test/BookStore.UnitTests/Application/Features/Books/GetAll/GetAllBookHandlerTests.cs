using BookStore.Application.Books.GetAll;
using BookStore.Domain.Catalog;
using Moq;

namespace BookStore.UnitTests.Application.Features.Books.GetAll;

public class GetAllBookHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    
    private readonly GetAllBooksQueryHandler _handler;
    
    public GetAllBookHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _handler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnBooks_WhenBooksExist()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book("Book 1", "Description 1", 10, "2008"),
            new Book("Book 2", "Description 2", 20, "2009"),
            new Book("Book 3", "Description 3", 30, "2010")
        };
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(books);
        
        var query = new GetAllBooksQuery();
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(books.Count, result.Value.Count());
        _bookRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }
}