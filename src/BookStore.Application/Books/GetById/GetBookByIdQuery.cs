using BookStore.Application.Books.Common;
using BookStore.Application.Messaging;

namespace BookStore.Application.Books.GetById;

public sealed record GetBookByIdQuery(int Id) : IQuery<BookResponse>;