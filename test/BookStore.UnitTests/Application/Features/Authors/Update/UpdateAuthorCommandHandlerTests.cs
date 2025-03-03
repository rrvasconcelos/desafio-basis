using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Update;
using BookStore.Domain.Authors;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.Update;

public class UpdateAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly UpdateAuthorCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateAuthorCommandHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateAuthorCommandHandler(_authorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAuthor_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateAuthorCommand(1,"Emmanuel");
        
        Author capturedAuthor = null!;
        
        _authorRepositoryMock
            .Setup(r => r.Update(It.IsAny<Author>()))
            .Callback<Author>(author => capturedAuthor = author);
        
        _authorRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new Author("Emmanuel"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Name, result.Value.Name);
        _authorRepositoryMock.Verify(r => r.Update(It.IsAny<Author>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedAuthor);
        Assert.Equal(command.Name, capturedAuthor.Name);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAuthorNotFound()
    {
        // Arrange
        var command = new UpdateAuthorCommand(1, "Emmanuel");
        
        _authorRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync((Author?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorError.NotFound, result.Error);
        _authorRepositoryMock.Verify(r => r.Update(It.IsAny<Author>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenAuthorNameNotUnique()
    {
        // Arrange
        var command = new UpdateAuthorCommand(1, "Emmanuel");
        
        _authorRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(),CancellationToken.None))
            .ReturnsAsync(new Author("Emmanuel"));

        _authorRepositoryMock
            .Setup(r => r.CheckIfAuthorNameExistsAsync(It.IsAny<string>(), It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorError.NameNotUnique, result.Error);
        _authorRepositoryMock.Verify(r => r.Update(It.IsAny<Author>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

