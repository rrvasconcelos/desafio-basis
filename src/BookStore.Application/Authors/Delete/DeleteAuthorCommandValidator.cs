using FluentValidation;

namespace BookStore.Application.Authors.Delete;

public class DeleteAuthorCommandValidator:AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author Id is required")
            .GreaterThan(0).WithMessage("Author Id is required");
    }
}