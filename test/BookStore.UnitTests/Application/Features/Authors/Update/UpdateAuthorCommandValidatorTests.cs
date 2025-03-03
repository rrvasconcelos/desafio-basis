using BookStore.Application.Authors.Update;
using BookStore.Domain.Authors;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Authors.Update;

public class UpdateAuthorCommandValidatorTests
{
    private readonly UpdateAuthorCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldNotHaveError_WhenNameIsValid()
    {
        // Arrange
        var command = new UpdateAuthorCommand(1, "John Doe");
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenNameIsEmptyOrWhitespace(string? invalidName)
    {
        // Arrange
        var command = new UpdateAuthorCommand(1, invalidName);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(AuthorValidationConstants.AuthorNameRequiredError);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void Validate_ShouldHaveError_WhenIdIsNullOrZero(int invalidId)
    {
        // Arrange
        var command = new UpdateAuthorCommand(invalidId, "John Doe");
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage(AuthorValidationConstants.AuthorIdRequiredError);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command = new UpdateAuthorCommand(1, new string('A', 201));
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(AuthorValidationConstants.AuthorNameLengthError);
    }
}