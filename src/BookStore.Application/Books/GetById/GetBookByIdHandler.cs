using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Books.GetById;

public class GetBookByIdHandler(IBookRepository bookRepository) : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
        {
            return Result.Failure<BookResponse>(BookError.NotFound);
        }

        return new BookResponse(
            book.Id,
            book.BookAuthors.Select(e => e.AuthorId),
            book.BookSubjects.Select(e=> e.SubjectId),
            book.Title,
            book.Publisher,
            book.Edition,
            book.PublicationYear,
            book.BookPrices.Select(e => new BookPriceResponse(e.Value, e.PurchaseType)),
            book.CreatedAt,
            book.Active);
    }
}