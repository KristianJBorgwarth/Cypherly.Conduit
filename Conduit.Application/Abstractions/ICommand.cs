using Conduit.Domain.Common;
using MediatR;

namespace Conduit.Application.Abstractions;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;