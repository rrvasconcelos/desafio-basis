using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Delete;
using BookStore.Domain.Catalog;
using Moq;

namespace BookStore.UnitTests.Application.Features.Books.Delete;

public class DeleteBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly DeleteBookCommandHandler _handler;

    public DeleteBookCommandHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteBook_WhenCommandIsValid()
    {
        // Arrange
        const int bookId = 1;
        var command = new DeleteBookCommand(bookId);

        var book = new Book("Clean Code", "Prentice Hall", 1, "2008", bookId);

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, CancellationToken.None))
            .ReturnsAsync(book);

        // Ação
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        const int bookId = 1;
        var command = new DeleteBookCommand(bookId);

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId, CancellationToken.None))
            .ReturnsAsync(default(Book));

        // Ação
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BookError.NotFound, result.Error);
    }
}