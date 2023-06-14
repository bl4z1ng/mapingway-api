using Mapingway.Common.Result;
using MediatR;

namespace Mapingway.Application.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}