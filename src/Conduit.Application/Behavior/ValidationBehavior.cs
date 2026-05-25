using Conduit.Domain.Common;
using FluentValidation;
using MediatR;

// ReSharper disable InvertIf

namespace Conduit.Application.Behavior;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return await next(cancellationToken);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next(cancellationToken);

        var errorMessage = string.Join("; ", validationResult.Errors.Select(e => $"{e.ErrorMessage} ({e.PropertyName})"));

        var error = Error.Validation("Validation failed: " + errorMessage);

        return ResultFactory.Fail<TResponse>(error);
    }
}
