using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Common;
using BookStore.Application.Books.Register;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using Moq;
using Xunit;

namespace BookStore.UnitTests.Application.Features.Books.Register;

public class RegisterBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock; 
    private readonly RegisterBookCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly int[] AuthorId = [1];
    private const int SubjectId = 1; 

    public RegisterBookCommandHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _subjectRepositoryMock = new Mock<ISubjectRepository>(); 
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        // Passar o repositório de assuntos para o handler
        _handler = new RegisterBookCommandHandler(
            _bookRepositoryMock.Object,
            _subjectRepositoryMock.Object, // Adicione essa linha
            _authorRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    private RegisterBookCommand CreateCommand(string title = "Clean Code", string publisher = "Prentice Hall", 
        int edition = 1, string publicationYear = "2008", decimal price = 29.99m, 
        PurchaseType purchaseType = PurchaseType.Online, int[] subjectsId = null)
    {
        return new RegisterBookCommand(
            title, 
            publisher, 
            edition, 
            publicationYear,
            [new PricesDto(price, purchaseType)], 
            subjectsId ?? new[] { SubjectId }, 
            AuthorId); 
    }

    [Fact]
    public async Task Handle_ShouldCreateBook_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand();

        var author = new Author(AuthorId[0], "Robert C. Martin");
        var subject = new Subject( "Software Engineering");

        _bookRepositoryMock.Setup(r => r.GetByTitleAsync(command.Title, CancellationToken.None))
            .ReturnsAsync(default(Book));

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(author);

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(subject);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Title, result.Value.Title);
        Assert.Equal(command.Publisher, result.Value.Publisher);
        Assert.Equal(command.Edition, result.Value.Edition);
        Assert.Equal(command.PublicationYear, result.Value.PublicationYear);
        _bookRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenTitleIsNotUnique()
    {
        // Arrange
        var command = CreateCommand();

        var existingBook = new Book("Clean Code", "Some Publisher", 1, "2008");
        _bookRepositoryMock.Setup(r => r.GetByTitleAsync(command.Title, CancellationToken.None))
            .ReturnsAsync(existingBook);

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(new Author(AuthorId[0], "Robert C. Martin"));

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(new Subject( "Software Engineering"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BookError.TitleNotUnique, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenAuthorIsNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(default(Author));

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(new Subject( "Software Engineering"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthorError.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSubjectIsNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(new Author(AuthorId[0], "Robert C. Martin"));

        _bookRepositoryMock.Setup(r => r.GetByTitleAsync(command.Title, CancellationToken.None))
            .ReturnsAsync(default(Book));

        // Configure o mock do repositório de assuntos para retornar null
        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(default(Subject));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NotFound("Subject.NotFound", "Subject not found"), result.Error);
    }
}
