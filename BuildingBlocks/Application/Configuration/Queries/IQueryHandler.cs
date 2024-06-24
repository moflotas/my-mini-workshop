﻿using BuildingBlocks.Application.Contracts;
using MediatR;

namespace BuildingBlocks.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;