using BookStore.Application.Abstractions.Data;
using BookStore.Domain.Authors;
using BookStore.SharedKernel;
using MediatR;

namespace BookStore.Application.Authors.Delete;

public class DeleteAuthorCommandHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAuthorCommand, Result>
{
    public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);

        if (author == null)
        {
            return Result.Failure(AuthorError.NotFound);
        }

        authorRepository.Delete(author);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}