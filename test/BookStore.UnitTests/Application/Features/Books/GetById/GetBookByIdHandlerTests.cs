using BookStore.Application.Books.GetById;
using BookStore.Domain.Catalog;
using Moq;

namespace BookStore.UnitTests.Application.Features.Books.GetById;

public class GetBookByIdHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly GetBookByIdHandler _handler;
    
    public GetBookByIdHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _handler = new GetBookByIdHandler(_bookRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var book = new Book("Clean Code", "Robert C. Martin", 1, "2009");
        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(book.Id, CancellationToken.None))
            .ReturnsAsync(book);

        var query = new GetBookByIdQuery(book.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(book.Title, result.Value.Title);
        _bookRepositoryMock.Verify(r => r.GetByIdAsync(book.Id, CancellationToken.None), Times.Once);
    }
}