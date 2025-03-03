using FluentValidation;

namespace BookStore.Application.Subjects.Register;

public class RegisterSubjectCommandValidator : AbstractValidator<RegisterSubjectCommand>
{
    public RegisterSubjectCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(20).WithMessage("Description must not exceed 20 characters");
    }
}