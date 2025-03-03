using BookStore.Application.Messaging;
using BookStore.Application.Subjects.Common;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Subjects.GetAll;

public sealed record GetSubjectAllQuery : IQuery<IEnumerable<SubjectResponse>>;

public sealed class GetSubjectAllQueryHandler(ISubjectRepository subjectRepository)
    : IQueryHandler<GetSubjectAllQuery, IEnumerable<SubjectResponse>>
{
    public async Task<Result<IEnumerable<SubjectResponse>>> Handle(GetSubjectAllQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetAllAsync(cancellationToken);

        var result = subjects.Select(s => new SubjectResponse(s.Id, s.Description, s.Active));

        return result.ToList();
    }
}