﻿using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Abstractions.Messaging.Command;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> 
    where TCommand : ICommand<Result>
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}