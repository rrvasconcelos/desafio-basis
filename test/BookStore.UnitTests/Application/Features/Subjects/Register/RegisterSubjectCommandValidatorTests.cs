using BookStore.Application.Subjects.Register;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Subjects.Register;

public class RegisterSubjectCommandValidatorTests
{
    private readonly RegisterSubjectCommandValidator _validator = new();

    [Fact]
    public void GivenValidSubjectCommand_WhenValidate_ShouldNotHaveError()
    {
        // Arrange
        var subject = new RegisterSubjectCommand("Description Test");

        // Act
        var result = _validator.TestValidate(subject);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GivenInvalidSubjectCommand_WhenValidate_ShouldHaveError(string? invalidDescription)
    {
        // Arrange
        var subject = new RegisterSubjectCommand(invalidDescription);

        // Act
        var result = _validator.TestValidate(subject);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var subject = new RegisterSubjectCommand(new string('A', 21));

        // Act
        var result = _validator.TestValidate(subject);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must not exceed 20 characters");
    }
}