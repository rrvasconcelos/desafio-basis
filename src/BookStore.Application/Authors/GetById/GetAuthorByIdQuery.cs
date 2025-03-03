using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Authors.GetById;

public sealed record GetAuthorByIdQuery(int Id) : IQuery<AuthorResponse>;