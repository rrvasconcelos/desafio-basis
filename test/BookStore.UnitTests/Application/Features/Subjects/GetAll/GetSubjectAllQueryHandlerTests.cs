using BookStore.Application.Subjects.GetAll;
using BookStore.Domain.Catalog;
using FluentAssertions;
using Moq;

namespace BookStore.UnitTests.Application.Features.Subjects.GetAll;

public class GetSubjectAllQueryHandlerTests
{
    private readonly Mock<ISubjectRepository> _mockSubjectRepository;
    private readonly GetSubjectAllQueryHandler _handler;
    
    public GetSubjectAllQueryHandlerTests()
    {
        _mockSubjectRepository = new Mock<ISubjectRepository>();
        _handler = new GetSubjectAllQueryHandler(_mockSubjectRepository.Object);
    }
    
    [Fact]
    public async Task Handle_WhenCalled_ReturnsAllSubjects()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject("Subject 1"),
            new Subject("Subject 2")
        };
        _mockSubjectRepository.Setup(x => x.GetAllAsync(CancellationToken.None)).ReturnsAsync(subjects);
        
        // Act
        var result = await _handler.Handle(new GetSubjectAllQuery(), CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Count().Should().Be(2);
    }
}