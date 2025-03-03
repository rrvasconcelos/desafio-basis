using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Publishing;

namespace BookStore.Application.Books.Update;

public sealed record UpdateBookCommand(
    int Id,
    string Title,
    string Publisher,
    int Edition,
    string PublicationYear,
    List<PricesDto> Prices,
    int[] SubjectsId,
    int[] AuthorsId) : ICommand<BookResponse>;