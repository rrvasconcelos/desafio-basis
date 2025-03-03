using FluentValidation;

namespace BookStore.Application.Books.Register;

public class RegisterBookCommandValidator : AbstractValidator<RegisterBookCommand>
{
    public RegisterBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(40).WithMessage("Title must not exceed 40 characters");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher name is required")
            .MaximumLength(40).WithMessage("Publisher name must not exceed 40 characters");

        RuleFor(x => x.Edition)
            .GreaterThan(0).WithMessage("Edition must be greater than 0");

        RuleFor(x => x.PublicationYear)
            .NotEmpty().WithMessage("Publication year must be a valid year")
            .Matches(@"^\d{4}$").WithMessage("Publication year must be a valid year");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required")
            .Must(prices => prices is {Count: > 0}).WithMessage("At least one price is required")
            .ForEach(priceRule => priceRule
                .Must(priceDto => priceDto.Price > 0).WithMessage("Price must be greater than zero")
            );

        RuleFor(x => x.AuthorsId)
            .Must(x => x is { Length: > 0 }).WithMessage("Author ID is required")
            .ForEach(authorRule => authorRule
                .GreaterThan(0).WithMessage("Author ID must be greater than zero")
            );

        RuleFor(x => x.SubjectsId)
            .Must(x => x is { Length: > 0 }).WithMessage("At least one subject is required")
            .ForEach(subject => subject
                .GreaterThan(0).WithMessage("At least one subject is required")
            );
    }
}