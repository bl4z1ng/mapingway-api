using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Abstractions.Messaging.Command;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}