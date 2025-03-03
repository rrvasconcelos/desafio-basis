using BookStore.Application.Abstractions.Data;
using BookStore.Application.Messaging;
using BookStore.Domain.Catalog;
using BookStore.SharedKernel;

namespace BookStore.Application.Books.Delete;

public sealed class DeleteBookCommandHandler(IBookRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteBookCommand>
{
    public async Task<Result> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var book = await repository.GetByIdAsync(command.BookId, cancellationToken);

        if (book is null)
        {
            return Result.Failure(BookError.NotFound);
        }

        book.Delete();
        repository.Update(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}