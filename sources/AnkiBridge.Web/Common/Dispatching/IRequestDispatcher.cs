using MediatR;

namespace AnkiBridge.Web.Common.Dispatching;

public interface IRequestDispatcher
{
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);

    Task Send(
        IRequest request,
        CancellationToken cancellationToken = default);
}
