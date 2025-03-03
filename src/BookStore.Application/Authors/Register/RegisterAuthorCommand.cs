using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Authors.Register;

public sealed record RegisterAuthorCommand(string Name) : ICommand<AuthorResponse>;