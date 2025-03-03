using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;

namespace BookStore.Application.Authors.GetAll;

public sealed class GetAllAuthorsQueryHandler(IAuthorRepository authorRepository)
    : IQueryHandler<GetAllAuthorsQuery, List<AuthorResponse>>
{
    public async Task<Result<List<AuthorResponse>>> Handle(GetAllAuthorsQuery query,
        CancellationToken cancellationToken)
    {
        var authors = await authorRepository.GetAllAsync(cancellationToken);

        var response = authors
            .Select(author => new AuthorResponse(author.Id, author.Name))
            .ToList();

        return response;
    }
}