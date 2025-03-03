using BookStore.Application.Abstractions.Data;
using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Subjects.Update;

public sealed class UpdateSubjectCommandHandler(ISubjectRepository subjectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateSubjectCommand, SubjectResponse>
{
    public async Task<Result<SubjectResponse>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await subjectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subject is null)
        {
            return Result.Failure<SubjectResponse>(Error.NotFound("Subject.NotFound", "Subject not found."));
        }
        
        if (await subjectRepository.CheckIfDescriptionExistsAsync(request.Description, request.Id, cancellationToken))
        {
            return Result.Failure<SubjectResponse>(Error.Conflict("DuplicateSubject", "Subject with same description already exists."));
        }

        subject.UpdateDescription(request.Description);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SubjectResponse(subject.Id, subject.Description, subject.Active);
    }
}