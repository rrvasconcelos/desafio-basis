using BookStore.SharedKernel;
using MediatR;

namespace BookStore.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;