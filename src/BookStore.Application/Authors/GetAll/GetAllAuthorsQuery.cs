using BookStore.Application.Authors.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Authors.GetAll;

public sealed record GetAllAuthorsQuery: IQuery<List<AuthorResponse>>;