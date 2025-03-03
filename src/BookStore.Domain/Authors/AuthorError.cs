using BookStore.SharedKernel;

namespace BookStore.Domain.Authors;

public static class AuthorError
{
    public static readonly Error NameNotUnique = Error.Conflict("DuplicateAuthor", "Author name already exists");
    public static readonly Error NotFound = Error.NotFound("NotFoundAuthor", "Author not found");
}

