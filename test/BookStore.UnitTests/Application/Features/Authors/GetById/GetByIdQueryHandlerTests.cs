using BookStore.Application.Authors.GetById;
using BookStore.Domain.Authors;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.GetById;

public class GetByIdQueryHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly GetAuthorByIdQueryHandler _handler;
    
    public GetByIdQueryHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _handler = new GetAuthorByIdQueryHandler(_authorRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
    {
        // Arrange
        var author = new Author("Emmanuel");
        _authorRepositoryMock
            .Setup(r => r.GetByIdAsync(author.Id, CancellationToken.None))
            .ReturnsAsync(author);

        var query = new GetAuthorByIdQuery(author.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(author.Id, result.Value.Id);
        Assert.Equal(author.Name, result.Value.Name);
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(author.Id, CancellationToken.None), Times.Once);
    }
}