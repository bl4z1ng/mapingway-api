using Mapingway.SharedKernel.Result;
using MediatR;

namespace Mapingway.Application.Contracts.Abstractions.Messaging.Command;

public interface ICommand : IRequest<Result>
{}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{}