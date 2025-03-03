using BookStore.Domain.Publishing;

namespace BookStore.Application.Books.Common;

public sealed record PricesDto(decimal Price, PurchaseType PurchaseType);