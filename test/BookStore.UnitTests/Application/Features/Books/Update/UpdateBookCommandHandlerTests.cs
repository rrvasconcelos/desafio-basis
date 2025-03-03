using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Common;
using BookStore.Application.Books.Update;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using Moq;
using Xunit;

namespace BookStore.UnitTests.Application.Features.Books.Update;

public class UpdateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock; 
    private readonly UpdateBookCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly int[] AuthorId = [1];
    private const int BookId = 1;
    private const int SubjectId = 1; 

    public UpdateBookCommandHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateBookCommandHandler(
            _bookRepositoryMock.Object,
            _subjectRepositoryMock.Object,
            _authorRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    private UpdateBookCommand CreateCommand(string title = "Clean Code", string publisher = "Prentice Hall",
        int edition = 1, string publicationYear = "2008", decimal? price = 29.99m,
        PurchaseType purchaseType = PurchaseType.Online, int[] authorId = null, int subjectId = SubjectId)
    {
        authorId ??= AuthorId;
        
        return new UpdateBookCommand(
            BookId, 
            title, 
            publisher, 
            edition, 
            publicationYear, 
            [new PricesDto(price.Value, purchaseType)],
            [subjectId], 
            authorId);
    }

    [Fact]
    public async Task Handle_ShouldUpdateBook_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand();

        var author = new Author(AuthorId[0], "Robert C. Martin");
        var existingBook = new Book("Old Title", "Old Publisher", 1, "2000", BookId);

        existingBook.AddPrice(PurchaseType.Online, 29.99m);
        var subject = new Subject("Software Engineering", 1);

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(author);

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(BookId, CancellationToken.None))
            .ReturnsAsync(existingBook);

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
        _bookRepositoryMock.Verify(r => r.Update(It.IsAny<Book>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenAuthorIsNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(default(Author));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthorError.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBookIsNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(new Author(AuthorId[0], "Robert C. Martin"));

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(BookId, CancellationToken.None))
            .ReturnsAsync(default(Book)); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BookError.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSubjectIsNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _authorRepositoryMock.Setup(r => r.GetByIdAsync(AuthorId[0], CancellationToken.None))
            .ReturnsAsync(new Author(AuthorId[0], "Robert C. Martin"));

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(BookId, CancellationToken.None))
            .ReturnsAsync(new Book("Old Title", "Old Publisher", 1, "2000", BookId));

        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(SubjectId, CancellationToken.None))
            .ReturnsAsync(default(Subject)); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NotFound("Subject.NotFound", "Subject not found"), result.Error);
    }
}