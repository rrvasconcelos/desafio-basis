using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Books.Register;

public sealed record RegisterBookCommand(
    string Title,
    string Publisher,
    int Edition,
    string PublicationYear,
    List<PricesDto> Price,
    int[] SubjectsId,
    int[] AuthorsId)
    : ICommand<BookResponse>;