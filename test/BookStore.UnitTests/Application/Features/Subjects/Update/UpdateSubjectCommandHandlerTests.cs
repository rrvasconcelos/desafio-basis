using BookStore.Application.Abstractions.Data;
using BookStore.Application.Subjects.Update;
using BookStore.Domain.Catalog;
using Moq;

namespace BookStore.UnitTests.Application.Features.Subjects.Update;

public class UpdateSubjectCommandHandlerTests
{
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;

    private readonly UpdateSubjectCommandHandler _handler;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateSubject_WhenCommandIsValid()
    {
        // Arrange
        const int id = 1;
        var command = new UpdateSubjectCommand(id, "Clean Code");
        var subject = new Subject("Clean Code");

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(subject);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Description, result.Value.Description);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handler_ShouldReturnError_WhenAuthorDescriptionAlreadyExists()
    {
        // Arrange
        const int id = 1;
        var command = new UpdateSubjectCommand(id, "Clean Code");
        var existingSubject = new Subject("Clean Code");

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(existingSubject);

        _subjectRepositoryMock.Setup(r =>
                r.CheckIfDescriptionExistsAsync(command.Description, command.Id, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DuplicateSubject", result.Error.Code);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        
    }
    
    [Fact]
    public async Task Handler_ShouldReturnError_WhenSubjectNotFound()
    {
        // Arrange
        const int id = 1;
        var command = new UpdateSubjectCommand(id, "Clean Code");

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(default(Subject));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Subject.NotFound", result.Error.Code);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}