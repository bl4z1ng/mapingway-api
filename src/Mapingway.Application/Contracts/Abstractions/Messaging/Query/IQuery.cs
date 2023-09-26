using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Contracts.Abstractions.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}