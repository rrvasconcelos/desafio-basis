using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Subjects.GetById;

public sealed class GetSubjectByIdQueryHandler(ISubjectRepository subjectRepository)
    : IQueryHandler<GetSubjectByIdQuery, SubjectResponse>
{
    public async Task<Result<SubjectResponse>> Handle(GetSubjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var subject = await subjectRepository.GetByIdAsync(request.Id, cancellationToken);

        return subject is null
            ? Result.Failure<SubjectResponse>(Error.NotFound("Subject.NotFound", "Subject not found."))
            : new SubjectResponse(subject.Id, subject.Description, subject.Active);
    }
}