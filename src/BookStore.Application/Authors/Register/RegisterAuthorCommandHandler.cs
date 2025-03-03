using BookStore.Application.Abstractions.Data;
using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;

namespace BookStore.Application.Authors.Register;

public class RegisterAuthorCommandHandler(IAuthorRepository context, IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterAuthorCommand, AuthorResponse>
{
    public async Task<Result<AuthorResponse>> Handle(RegisterAuthorCommand request, CancellationToken cancellationToken)
    {
        if (await context.GetByNameAsync(request.Name, cancellationToken) != null)
        {
            return Result.Failure<AuthorResponse>(AuthorError.NameNotUnique);
        }

        var author = new Author(request.Name);

        context.Add(author);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthorResponse(author.Id, author.Name);
    }
}