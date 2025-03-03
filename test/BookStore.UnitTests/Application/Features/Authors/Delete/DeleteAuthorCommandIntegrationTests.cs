using BookStore.Application.Abstractions.Behaviors;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Delete;
using BookStore.Domain.Authors;
using FluentValidation;
using MediatR;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.Delete;

public class DeleteAuthorCommandIntegrationTests
{
    private readonly DeleteAuthorCommandHandler _handler;

    public DeleteAuthorCommandIntegrationTests()
    {
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteAuthorCommandHandler(authorRepositoryMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidationBehavior_ShouldReturnValidationError_WhenIdIsInvalid()
    {
        // Arrange
        var command = new DeleteAuthorCommand(1);
        var validators = new List<IValidator<DeleteAuthorCommand>>
        {
            new DeleteAuthorCommandValidator()
        };

        var validationBehavior = new ValidationPipelineBehavior<DeleteAuthorCommand, Result>(validators);

        RequestHandlerDelegate<Result> next = () => _handler.Handle(command, CancellationToken.None);

        // Act
        var result = await validationBehavior.Handle(command, next, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
}