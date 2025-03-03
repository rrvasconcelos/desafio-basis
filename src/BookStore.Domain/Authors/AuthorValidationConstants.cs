namespace BookStore.Domain.Authors;

public static class AuthorValidationConstants
{
    public const int AuthorNameMaxLength = 40;

    public static readonly string AuthorNameLengthError = $"Author name must not exceed {AuthorNameMaxLength} characters";
    public static readonly string AuthorNameRequiredError = "Author name is required";
    public static readonly string AuthorIdRequiredError = "Author id is required";
    public static readonly string AuthorIdInvalidError = "Author id is invalid";
}
