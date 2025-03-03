using BookStore.Domain.Publishing;

namespace BookStore.Application.Books.Common;

public sealed record BookResponse(
    int Id,
    IEnumerable<int> AuthorIds,
    IEnumerable<int> SubjectsId,
    string Title,
    string Publisher,
    int Edition,
    string PublicationYear,
    IEnumerable<BookPriceResponse> BookPrices,
    DateTimeOffset CreatedAt,
    bool Active);
    
    public record BookPriceResponse(decimal Value, PurchaseType PurchaseType);