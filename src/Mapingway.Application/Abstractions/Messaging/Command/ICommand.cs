﻿using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Messaging.Command;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}