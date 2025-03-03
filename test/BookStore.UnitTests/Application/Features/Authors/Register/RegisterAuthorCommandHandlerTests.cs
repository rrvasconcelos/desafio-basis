using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Register;
using BookStore.Domain.Authors;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.Register;

public class RegisterAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly RegisterAuthorCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    public RegisterAuthorCommandHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterAuthorCommandHandler(_authorRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldCreateAuthor_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterAuthorCommand("Emmanuel");
        Author capturedAuthor = null!;

        _authorRepositoryMock
            .Setup(r => r.Add(It.IsAny<Author>()))
            .Callback<Author>(author => capturedAuthor = author);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Name, result.Value.Name);
        _authorRepositoryMock.Verify(r => r.Add(It.IsAny<Author>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedAuthor);
        Assert.Equal(command.Name, capturedAuthor.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationFailure_WhenAuthorNameAlreadyExists()
    {
        // Arrange
        var existingAuthor = new Author("Emmanuel");
        _authorRepositoryMock
            .Setup(r => r.GetByNameAsync(existingAuthor.Name, CancellationToken.None))
            .ReturnsAsync(existingAuthor);

        var command = new RegisterAuthorCommand(existingAuthor.Name);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(Error.Conflict("DuplicateAuthor", "Author name already exists"), result.Error);

        _authorRepositoryMock.Verify(r => r.Add(It.IsAny<Author>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}