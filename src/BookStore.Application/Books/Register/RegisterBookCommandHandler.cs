using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Books.Register;

public sealed class RegisterBookCommandHandler(
    IBookRepository repository, 
    ISubjectRepository subjectRepository,
    IAuthorRepository authorRepository,  
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterBookCommand, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(RegisterBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book(request.Title, request.Publisher, request.Edition, request.PublicationYear);
        
        var existingBook = await repository.GetByTitleAsync(request.Title, cancellationToken);
        
        if (existingBook is not null)
        {
            return Result.Failure<BookResponse>(BookError.TitleNotUnique);
        }

        var authors = new List<Author>();
        foreach (var id in request.AuthorsId)
        {
            var author = await authorRepository.GetByIdAsync(id, cancellationToken);
        
            if (author is null)
            {
                return Result.Failure<BookResponse>(AuthorError.NotFound);
            }
            
            authors.Add(author);
        }
        
        book.AddAuthors(authors);

        foreach (var price in request.Price)
        {
            book.AddPrice(price.PurchaseType, price.Price);
        }

        var resultSubjects = await AssignSubjects(request, cancellationToken, book);

        if (!resultSubjects.IsSuccess)
        {
            return Result.Failure<BookResponse>(resultSubjects.Error);
        }

        await repository.AddAsync(book);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new BookResponse(
            book.Id, 
            request.SubjectsId,
            request.AuthorsId, 
            book.Title, 
            book.Publisher,
            book.Edition, 
            book.PublicationYear, 
            book.BookPrices.Select(e=> new BookPriceResponse(e.Value, e.PurchaseType)),
            book.CreatedAt, 
            book.Active);
    }

    private async Task<Result> AssignSubjects(RegisterBookCommand request, CancellationToken cancellationToken, Book book)
    {
        var subjects = new List<Subject>();
        foreach (var id in request.SubjectsId)
        {
            var subject = await subjectRepository.GetByIdAsync(id, cancellationToken);
            
            if (subject is null)
            {
                return Result.Failure(Error.NotFound("Subject.NotFound", "Subject not found"));
            }
            subjects.Add(subject);
        }
        
        book.AddSubjects(subjects);
        
        return Result.Success();
    }
}