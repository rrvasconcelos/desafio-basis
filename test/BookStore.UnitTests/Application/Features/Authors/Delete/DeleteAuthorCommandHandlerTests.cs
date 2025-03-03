using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Delete;
using BookStore.Domain.Authors;
using FluentAssertions;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.Delete;

public class DeleteAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> _mockAuthorRepository;
    private readonly DeleteAuthorCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public DeleteAuthorCommandHandlerTests()
    {
        _mockAuthorRepository = new Mock<IAuthorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteAuthorCommandHandler(_mockAuthorRepository.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidAuthorId_ShouldDeleteAuthor()
    {
        // Arrange
        const int authorId = 0;
        var author = new Author("user");

        _mockAuthorRepository.Setup(x => x.GetByIdAsync(authorId, CancellationToken.None))
            .ReturnsAsync(author);

        _mockAuthorRepository.Setup(x => x.Delete(author));

        var command = new DeleteAuthorCommand(authorId);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidAuthorId_ShouldReturnNotFound()
    {
        // Arrange
        const int authorId = 0;
        _mockAuthorRepository.Setup(x => x.GetByIdAsync(authorId, CancellationToken.None))
            .ReturnsAsync(default(Author));

        var command = new DeleteAuthorCommand(authorId);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Error.Description.Should().Be("Author not found");
    }
}