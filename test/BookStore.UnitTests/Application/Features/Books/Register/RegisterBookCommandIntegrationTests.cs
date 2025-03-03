using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Common;
using BookStore.Application.Books.Register;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using Moq;
using Xunit;

namespace BookStore.UnitTests.Application.Features.Books.Register;

public class RegisterBookCommandIntegrationTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterBookCommandHandler _handler;

    private readonly int[] AuthorId = [1];
    private const int SubjectId = 1;

    public RegisterBookCommandIntegrationTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RegisterBookCommandHandler(
            _bookRepositoryMock.Object,
            _subjectRepositoryMock.Object, // Aqui você deve passar o mock do repositório de assuntos.
            _authorRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateBook_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterBookCommand(
            "Clean Code",
            "Prentice Hall",
            1,
            "2008",
            [new PricesDto(29.99m, PurchaseType.Online)],
            new[] { SubjectId },
            AuthorId);

        var author = new Author(AuthorId[0], "Robert C. Martin");
        var subject = new Subject("Software Engineering"); // Cria um Subject com descrição

        _bookRepositoryMock.Setup(r => r.GetByTitleAsync(command.Title, CancellationToken.None))
            .ReturnsAsync(default(Book));

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(author);

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(subject); // Simula que o assunto existe

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Title, result.Value.Title);
        _bookRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSubjectNotFound()
    {
        // Arrange
        var command = new RegisterBookCommand(
            "Clean Code",
            "Prentice Hall",
            1,
            "2008",
            [new PricesDto(29.90m, PurchaseType.Online)],
            [SubjectId],
            AuthorId);

        _bookRepositoryMock.Setup(r => r.GetByTitleAsync(command.Title, CancellationToken.None))
            .ReturnsAsync(default(Book));

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(new Author(AuthorId[0], "Robert C. Martin"));

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(default(Subject)); // Simula a ausência do assunto

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NotFound("Subject.NotFound", "Subject not found"), result.Error);
    }
}