using BookStore.Application.Books.Common;
using BookStore.Application.Books.Register;
using BookStore.Domain.Publishing;
using FluentValidation.TestHelper;
using Xunit;

namespace BookStore.UnitTests.Application.Features.Books.Register;

public class RegisterBookCommandValidatorTests
{
    private readonly RegisterBookCommandValidator _validator = new();

    private static readonly int[] DefaultSubjectsId = { 1 }; // ID padrão do assunto
    private static readonly int[] DefaultAuthorId = [1];

    private RegisterBookCommand CreateCommand(string title = "Clean Code", string publisher = "Prentice Hall",
        int edition = 1, string publicationYear = "2008", decimal price = 29.99m,
        PurchaseType purchaseType = PurchaseType.Online, int[] subjectsId = null,
        int[] authorId = null) // Mude para null
    {
        authorId ??= DefaultAuthorId;

        return new RegisterBookCommand(
            title,
            publisher,
            edition,
            publicationYear,
            new List<PricesDto> { new PricesDto(price, purchaseType) }, // Use List<PricesDto> em vez de array
            subjectsId ?? DefaultSubjectsId, // Usa o ID padrão se nenhum for passado
            authorId); // Permite passar um AuthorId específico
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenTitleIsEmpty(string? invalidTitle)
    {
        // Arrange
        var command = CreateCommand(invalidTitle);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenTitleIsTooLong()
    {
        // Arrange
        var command = CreateCommand(title: new string('A', 41));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must not exceed 40 characters");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenTitleIsValid()
    {
        // Act
        var result = _validator.TestValidate(CreateCommand());

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenPublisherIsEmpty(string? invalidPublisher)
    {
        // Arrange
        var command = CreateCommand(publisher: invalidPublisher);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Publisher)
            .WithErrorMessage("Publisher name is required");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPublisherIsTooLong()
    {
        // Arrange
        var command = CreateCommand(publisher: new string('A', 41));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Publisher)
            .WithErrorMessage("Publisher name must not exceed 40 characters");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenPublisherIsValid()
    {
        // Act 
        var result = _validator.TestValidate(CreateCommand());

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Publisher);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEditionIsZero()
    {
        // Arrange
        var command = CreateCommand(edition: 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Edition)
            .WithErrorMessage("Edition must be greater than 0");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenEditionIsValid()
    {
        // Act
        var result = _validator.TestValidate(CreateCommand());

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Edition);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("200")]
    [InlineData("20000")]
    [InlineData("20A0")]
    public void Validate_ShouldHaveError_WhenPublicationYearIsInvalid(string? invalidPublicationYear)
    {
        // Arrange
        var command = CreateCommand(publicationYear: invalidPublicationYear);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublicationYear)
            .WithErrorMessage("Publication year must be a valid year");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenPublicationYearIsValid()
    {
        // Act
        var result = _validator.TestValidate(CreateCommand());

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublicationYear);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenCommandIsValid()
    {
        // Act
        var command = CreateCommand();
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCommandIsInvalid()
    {
        // Arrange
        var command = CreateCommand("", "", 0, "", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCommandIsInvalidAndContainMultipleErrors()
    {
        // Arrange
        var command = new RegisterBookCommand(
            string.Empty,
            string.Empty,
            0,
            string.Empty,
            [new PricesDto(0, PurchaseType.Online)],
            [], // Passa um array vazio,
            []); // AuthorId inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Publisher);
        result.ShouldHaveValidationErrorFor(x => x.Edition);
        result.ShouldHaveValidationErrorFor(x => x.PublicationYear);
        result.ShouldHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.AuthorsId); // Verifica AuthorId
        result.ShouldHaveValidationErrorFor(x => x.SubjectsId); // Verifica SubjectsId
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPriceIsZeroOrNegative()
    {
        // Arrange
        var command = CreateCommand(price: 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Price must be greater than zero");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenAuthorIdIsEmpty()
    {
        // Arrange
        var command = CreateCommand(authorId: [0]); // Muda para 0 para verificar a validação

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AuthorsId)
            .WithErrorMessage("Author ID must be greater than zero");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenSubjectsIdIsEmpty()
    {
        // Arrange
        var command = CreateCommand(subjectsId: new int[0]); // Passa um array vazio

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SubjectsId)
            .WithErrorMessage("At least one subject is required");
    }
}