using FluentValidation;

namespace BookStore.Application.Subjects.Update;

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotEqual(0).WithMessage("Id is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(20).WithMessage("Description must not exceed 20 characters");
    }
}