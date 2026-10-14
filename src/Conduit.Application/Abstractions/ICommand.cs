using Conduit.Domain.Common;
using Mediator;

namespace Conduit.Application.Abstractions;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;