using BookStore.Application.Books.Delete;
using FluentValidation.TestHelper;

namespace BookStore.UnitTests.Application.Features.Books.Delete;

public class DeleteBookCommandTests
{
    private readonly DeleteBookCommandValidator _validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenBookIdIsNotValid(int id)
    {
        // Arrange
        var command = new DeleteBookCommand(id);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookId)
            .WithErrorMessage("BookId is required");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenBookIdIsValid()
    {
        // Arrange
        var command = new DeleteBookCommand(1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.BookId);
    }
}