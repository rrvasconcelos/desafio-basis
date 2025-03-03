using BookStore.Domain.Authors;
using FluentValidation;

namespace BookStore.Application.Authors.Register;

public class RegisterAuthorCommandValidator: AbstractValidator<RegisterAuthorCommand>
{
    public RegisterAuthorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(AuthorValidationConstants.AuthorNameRequiredError)
            .MaximumLength(AuthorValidationConstants.AuthorNameMaxLength).WithMessage(AuthorValidationConstants.AuthorNameLengthError);
    }
}