using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Authors.Update;

public sealed record UpdateAuthorCommand(int Id, string Name) : ICommand<AuthorResponse>;