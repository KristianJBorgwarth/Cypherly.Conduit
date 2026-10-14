using Conduit.Domain.Common;
using Mediator;

namespace Conduit.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }