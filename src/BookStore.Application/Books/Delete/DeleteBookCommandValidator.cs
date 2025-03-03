using FluentValidation;

namespace BookStore.Application.Books.Delete;

public sealed class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("BookId is required")
            .GreaterThan(0).WithMessage("BookId is required");
    }
}