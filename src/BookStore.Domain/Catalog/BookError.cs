using BookStore.SharedKernel;

namespace BookStore.Domain.Catalog;

public static class BookError
{
    public static readonly Error TitleNotUnique = Error.Conflict("DuplicateBook", "Book with the same title already exists");
    public static readonly Error NotFound = Error.NotFound("NotFoundBook", "Book not found");
    public static readonly Error InvalidPrice = Error.BadRequest("InvalidPrice", "Price must be greater than zero");

}