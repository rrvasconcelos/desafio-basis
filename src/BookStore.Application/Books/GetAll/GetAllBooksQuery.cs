using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Books.GetAll;

public sealed record GetAllBooksQuery : IQuery<IEnumerable<BookResponse>>;

public sealed class GetAllBooksQueryHandler(IBookRepository bookRepository)
    : IQueryHandler<GetAllBooksQuery, IEnumerable<BookResponse>>
{
    public async Task<Result<IEnumerable<BookResponse>>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);

        var response = books
            .Select(book => 
                new BookResponse(
                    book.Id,
                    book.BookAuthors.Select(e=> e.AuthorId),
                    book.BookSubjects.Select(e=> e.SubjectId),
                    book.Title,
                    book.Publisher,
                    book.Edition,
                    book.PublicationYear,
                    book.BookPrices.Select(e => new BookPriceResponse(e.Value, e.PurchaseType)),
                    book.CreatedAt,
                    book.Active))
            .ToList();

        return response;
    }
}