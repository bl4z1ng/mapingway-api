using Mapingway.SharedKernel.Result;
using MediatR;

namespace Mapingway.Application.Contracts.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{}