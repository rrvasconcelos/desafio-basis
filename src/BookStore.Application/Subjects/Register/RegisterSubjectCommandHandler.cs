using BookStore.Application.Abstractions.Data;
using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Subjects.Register;

public class RegisterSubjectCommandHandler(ISubjectRepository subjectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterSubjectCommand, SubjectResponse>
{
    public async Task<Result<SubjectResponse>> Handle(RegisterSubjectCommand request,
        CancellationToken cancellationToken)
    {
        var existingSubject = await subjectRepository.GetByDescriptionAsync(request.Description, cancellationToken);

        if (existingSubject != null)
            return Result.Failure<SubjectResponse>(Error.NotFound("DuplicateSubject",
                "Subject with the same description already exists"));

        var subject = new Subject(request.Description);

        subjectRepository.Add(subject);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SubjectResponse(subject.Id, subject.Description, subject.Active);
    }
}