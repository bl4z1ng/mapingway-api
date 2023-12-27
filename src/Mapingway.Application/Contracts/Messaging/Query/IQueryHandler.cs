using Mapingway.SharedKernel.Result;
using MediatR;

namespace Mapingway.Application.Contracts.Messaging.Query;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{}