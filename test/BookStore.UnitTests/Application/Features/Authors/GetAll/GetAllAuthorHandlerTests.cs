using BookStore.Application.Authors.GetAll;
using BookStore.Domain.Authors;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.GetAll;

public class GetAllAuthorHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly GetAllAuthorsQueryHandler _handler;

    public GetAllAuthorHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _handler = new GetAllAuthorsQueryHandler(_authorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthors_WhenAuthorsExist()
    {
        // Arrange
        var authors = new List<Author>
        {
            new Author("Emmanuel"),
            new Author("Chidi"),
            new Author("John")
        };
        _authorRepositoryMock
            .Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(authors);

        var query = new GetAllAuthorsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(authors.Count, result.Value.Count);
        _authorRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }
}