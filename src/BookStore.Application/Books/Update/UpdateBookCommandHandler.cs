using BookStore.Application.Abstractions.Data;
using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Books.Update;

public sealed class UpdateBookCommandHandler(
    IBookRepository bookRepository,
    ISubjectRepository subjectRepository,
    IAuthorRepository authorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateBookCommand, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
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

        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
        {
            return Result.Failure<BookResponse>(BookError.NotFound);
        }

        var subjects = new List<Subject>();
        foreach (var subjectId in request.SubjectsId)
        {
            var subject = await subjectRepository.GetByIdAsync(subjectId, cancellationToken);
            if (subject is null)
            {
                return Result.Failure<BookResponse>(Error.NotFound("Subject.NotFound", "Subject not found"));
            }

            subjects.Add(subject);
        }

        book.Update(request.Title, request.Publisher, request.Edition, request.PublicationYear);
        book.UpdateAuthors(authors);
        book.UpdateSubjects(subjects);

        var prices = request.Prices.Select(p => (p.PurchaseType, p.Price)).ToList();
        book.UpdatePrices(prices);

        bookRepository.Update(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new BookResponse(
            book.Id,
            book.BookSubjects.Select(e=> e.SubjectId),
            book.BookAuthors.Select(e => e.AuthorId),
            book.Title,
            book.Publisher,
            book.Edition,
            book.PublicationYear,
            book.BookPrices.Select(e => new BookPriceResponse(e.Value, e.PurchaseType)),
            book.CreatedAt,
            book.Active);
    }
}