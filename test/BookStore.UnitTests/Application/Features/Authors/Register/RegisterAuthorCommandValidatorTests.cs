using BookStore.Application.Authors.Register;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Authors.Register;

public class RegisterAuthorCommandValidatorTests
{
    private readonly RegisterAuthorCommandValidator _validator;

    public RegisterAuthorCommandValidatorTests()
    {
        _validator = new RegisterAuthorCommandValidator();
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenNameIsValid()
    {
        // Arrange
        var command = new RegisterAuthorCommand("John Doe");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string? invalidName)
    {
        // Arrange
        var command = new RegisterAuthorCommand(invalidName);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Author name is required");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command = new RegisterAuthorCommand(new string('A', 201));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Author name must not exceed 40 characters");
    }
}