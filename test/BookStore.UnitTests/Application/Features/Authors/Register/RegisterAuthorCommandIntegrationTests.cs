using BookStore.Application.Abstractions.Behaviors;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Common;
using BookStore.Application.Authors.Register;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;
using FluentValidation;
using MediatR;
using Moq;

namespace BookStore.UnitTests.Application.Features.Authors.Register;

public class RegisterAuthorCommandIntegrationTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterAuthorCommandHandler _handler;

    public RegisterAuthorCommandIntegrationTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterAuthorCommandHandler(_authorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidationBehavior_ShouldReturnValidationError_WhenNameIsInvalid()
    {
        // Arrange
        var command = new RegisterAuthorCommand("");
        var validators = new List<IValidator<RegisterAuthorCommand>>
        {
            new RegisterAuthorCommandValidator() // Certifique-se de que este validador exista e esteja correto
        };

        var validationBehavior = new ValidationPipelineBehavior<RegisterAuthorCommand, Result<AuthorResponse>>(validators);

        // Delegate para chamar o handler
        RequestHandlerDelegate<Result<AuthorResponse>> next = () => _handler.Handle(command, CancellationToken.None);

        // Act
        var result = await validationBehavior.Handle(command, next, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess); // Verifica se o resultado indica falha
        Assert.NotNull(result.Error); // Verifica se há um erro no resultado
    }
}

