using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;

namespace BookStore.Application.Authors.GetById;

public class GetAuthorByIdQueryHandler(IAuthorRepository repository): IQueryHandler<GetAuthorByIdQuery, AuthorResponse>
{
    public async Task<Result<AuthorResponse>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        return author is null ? 
            Result.Failure<AuthorResponse>(AuthorError.NotFound) : 
            new AuthorResponse(author.Id, author.Name);
    }
}