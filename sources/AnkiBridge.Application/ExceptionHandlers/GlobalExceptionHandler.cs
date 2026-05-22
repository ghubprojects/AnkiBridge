using AnkiBridge.Shared.Results;
using MediatR;
using MediatR.Pipeline;

namespace AnkiBridge.Application.ExceptionHandlers;

public class GlobalExceptionHandler<TRequest, TResponse, TException>
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<Result>
    where TResponse : Result
    where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var ex = (Exception)exception;
        while (ex.InnerException != null)
            ex = ex.InnerException;

        state.SetHandled((TResponse)Result.Failure(ex.Message));

        return Task.CompletedTask;
    }
}