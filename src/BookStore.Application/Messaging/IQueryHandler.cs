﻿using BookStore.SharedKernel;
using MediatR;

namespace BookStore.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;