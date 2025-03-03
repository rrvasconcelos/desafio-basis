using BookStore.Application.Messaging;

namespace BookStore.Application.Authors.Delete;

public sealed record DeleteAuthorCommand(int AuthorId): ICommand;