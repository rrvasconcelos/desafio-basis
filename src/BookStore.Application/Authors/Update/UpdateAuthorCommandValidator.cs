using BookStore.Domain.Authors;
using FluentValidation;

namespace BookStore.Application.Authors.Update;

public class UpdateAuthorCommandValidator: AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
                .WithMessage(AuthorValidationConstants.AuthorIdRequiredError)
                .NotEqual(0).WithMessage(AuthorValidationConstants.AuthorIdInvalidError);

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage(AuthorValidationConstants.AuthorNameRequiredError)
            .MaximumLength(AuthorValidationConstants.AuthorNameMaxLength)
                .WithMessage(AuthorValidationConstants.AuthorNameLengthError);
    }
}

