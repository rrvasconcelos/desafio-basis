using BookStore.Application.Subjects.Update;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Subjects.Update;

public class UpdateSubjectCommandValidatorTests
{
    private readonly UpdateSubjectCommandValidator _validator = new();
    
    [Fact]
    public void GivenValidSubjectCommand_WhenValidate_ShouldNotHaveError()
    {
        // Arrange
        var subject = new UpdateSubjectCommand(1, "Description Test");

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
        var subject = new UpdateSubjectCommand(1, invalidDescription);

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
        var subject = new UpdateSubjectCommand(1, new string('A', 21));

        // Act
        var result = _validator.TestValidate(subject);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must not exceed 20 characters");
    }
    
    [Fact]
    public void Validate_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var subject = new UpdateSubjectCommand(0, "Description Test");

        // Act
        var result = _validator.TestValidate(subject);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required");
    }
}