using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AnkiBridge.Application.Abstractions.IntegrationEvents;

public static class IntegrationEventSubscriptionExtensions
{
    public static IServiceCollection AddSubscription<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(
        this IServiceCollection services)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

        services.Configure<IntegrationEventSubscriptionInfo>(o =>
        {
            o.EventTypes[typeof(T).FullName!] = typeof(T);
        });

        return services;
    }
}