using FluentValidation;
using AnkiBridge.Shared.Results;
using MediatR;
using MediatR.Pipeline;

namespace AnkiBridge.Application.ExceptionHandlers;

public class ValidationExceptionHandler<TRequest, TResponse, TException>
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<Result>
    where TResponse : Result
    where TException : ValidationException
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var validationErrors = exception.Errors
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));

        state.SetHandled((TResponse)Result.ValidationFailure(validationErrors));

        return Task.CompletedTask;
    }
}
