using Conduit.Domain.Common;
using MediatR;

namespace Conduit.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }