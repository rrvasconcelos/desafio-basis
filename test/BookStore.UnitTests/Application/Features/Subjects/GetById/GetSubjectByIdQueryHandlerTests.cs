using BookStore.Application.Subjects.GetById;
using BookStore.Domain.Catalog;
using FluentAssertions;
using Moq;

namespace BookStore.UnitTests.Application.Features.Subjects.GetById;

public class GetSubjectByIdQueryHandlerTests
{
    private readonly Mock<ISubjectRepository> _mockSubjectRepository;
    private readonly GetSubjectByIdQueryHandler _handler;

    public GetSubjectByIdQueryHandlerTests()
    {
        _mockSubjectRepository = new Mock<ISubjectRepository>();
        _handler = new GetSubjectByIdQueryHandler(_mockSubjectRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsSubject()
    {
        // Arrange
        var subject = new Subject("Subject 1");
        _mockSubjectRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(subject);

        // Act
        var result = await _handler.Handle(new GetSubjectByIdQuery(1), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Description.Should().Be("Subject 1");
    }
    
    [Fact]
    public async Task Handle_WhenSubjectNotFound_ReturnsFailure()
    {
        // Arrange
        _mockSubjectRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(default(Subject));

        // Act
        var result = await _handler.Handle(new GetSubjectByIdQuery(1), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Subject.NotFound");
    }
}