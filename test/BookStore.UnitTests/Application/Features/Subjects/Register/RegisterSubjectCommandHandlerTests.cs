using BookStore.Application.Abstractions.Data;
using BookStore.Application.Subjects.Register;
using BookStore.Domain.Catalog;
using Moq;

namespace BookStore.UnitTests.Application.Features.Subjects.Register;

public class RegisterSubjectCommandHandlerTests
{
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    
    private readonly RegisterSubjectCommandHandler _handler;
    
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    public RegisterSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldCreateSubject_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterSubjectCommand("Clean Code");

        _subjectRepositoryMock.Setup(r => r.GetByDescriptionAsync(command.Description, CancellationToken.None))
            .ReturnsAsync((Subject)null!);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Description, result.Value.Description);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenSubjectWithSameDescriptionExists()
    {
        // Arrange
        var command = new RegisterSubjectCommand("Clean Code");
        var existingSubject = new Subject("Clean Code");

        _subjectRepositoryMock.Setup(r => r.GetByDescriptionAsync(command.Description, CancellationToken.None))
            .ReturnsAsync(existingSubject);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DuplicateSubject", result.Error.Code);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}