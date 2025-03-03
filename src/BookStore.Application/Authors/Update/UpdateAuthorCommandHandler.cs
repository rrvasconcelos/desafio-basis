using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;

namespace BookStore.Application.Authors.Update;

public class UpdateAuthorCommandHandler(IAuthorRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAuthorCommand, AuthorResponse>
{
    public async Task<Result<AuthorResponse>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (author == null)
        {
            return Result.Failure<AuthorResponse>(AuthorError.NotFound);
        }

        if (await repository.CheckIfAuthorNameExistsAsync(request.Name, request.Id, cancellationToken))
        {
            return Result.Failure<AuthorResponse>(AuthorError.NameNotUnique);
        }

        author.UpdateName(request.Name);

        repository.Update(author);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthorResponse(author.Id, author.Name);
    }
}