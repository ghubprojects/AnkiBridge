using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AnkiBridge.Application.Abstractions.Dispatching;

public sealed class RequestDispatcher(IServiceScopeFactory scopeFactory) : IRequestDispatcher
{
    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        return await sender.Send(request, cancellationToken);
    }

    public async Task Send(
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(request, cancellationToken);
    }
}