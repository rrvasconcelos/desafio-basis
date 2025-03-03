using BookStore.Application.Authors.Delete;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Authors.Delete;

public class DeleteAuthorCommandValidatorTests
{
    private readonly DeleteAuthorCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldNotHaveError_WhenAuthorIdIsValid()
    {
        // Arrange
        var command = new DeleteAuthorCommand(1);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AuthorId);
    }
    
    [Fact]
    public void Validate_ShouldHaveError_WhenAuthorIdIsEmpty()
    {
        // Arrange
        var command = new DeleteAuthorCommand(0);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AuthorId)
            .WithErrorMessage("Author Id is required");
    }
}