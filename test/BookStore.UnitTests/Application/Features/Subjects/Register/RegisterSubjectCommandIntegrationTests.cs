using BookStore.Application.Abstractions.Behaviors;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Subjects.Common;
using BookStore.Application.Subjects.Register;
using BookStore.Domain.Catalog;
using FluentValidation;
using MediatR;
using Moq;

namespace BookStore.UnitTests.Application.Features.Subjects.Register;

public class RegisterSubjectCommandIntegrationTests
{
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    
    private readonly RegisterSubjectCommandHandler _handler;
    
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    private readonly List<IValidator<RegisterSubjectCommand>> _validators;
    
    private ValidationPipelineBehavior<RegisterSubjectCommand, Result<SubjectResponse>> validationBehavior;
    
    public RegisterSubjectCommandIntegrationTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
        
        _validators = [new RegisterSubjectCommandValidator()];
        validationBehavior = new ValidationPipelineBehavior<RegisterSubjectCommand, Result<SubjectResponse>>(_validators);
    }

    [Fact]
    public async Task Handle_ShouldCreateSubject_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterSubjectCommand("Clean Code");
        
        _subjectRepositoryMock.Setup(r => r.GetByDescriptionAsync(command.Description, CancellationToken.None))
            .ReturnsAsync((Subject)null!);
        
        // Act
        RequestHandlerDelegate<Result<SubjectResponse>> next = () => _handler.Handle(command, CancellationToken.None);
        var result = await validationBehavior.Handle(command, next, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Error);
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
        RequestHandlerDelegate<Result<SubjectResponse>> next = () => _handler.Handle(command, CancellationToken.None);
        var result = await validationBehavior.Handle(command, next, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DuplicateSubject", result.Error.Code);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenDescriptionIsInvalid()
    {
        // Arrange
        var command = new RegisterSubjectCommand("");

        // Act
        RequestHandlerDelegate<Result<SubjectResponse>> next = () => _handler.Handle(command, CancellationToken.None);
        var result = await validationBehavior.Handle(command, next, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Validation.General", result.Error.Code);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}