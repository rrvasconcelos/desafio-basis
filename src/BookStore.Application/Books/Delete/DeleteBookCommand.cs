using BookStore.Application.Messaging;

namespace BookStore.Application.Books.Delete;

public sealed record DeleteBookCommand(int BookId) : ICommand;