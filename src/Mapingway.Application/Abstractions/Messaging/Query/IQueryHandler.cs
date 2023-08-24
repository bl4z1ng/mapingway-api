using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Abstractions.Messaging.Query;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}