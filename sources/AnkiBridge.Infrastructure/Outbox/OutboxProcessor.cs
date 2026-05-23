using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using AnkiBridge.Application.Common.IntegrationEvents;
using AnkiBridge.Application.Common.Contracts.Outbox;

namespace AnkiBridge.Infrastructure.Outbox;

public sealed class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    IOptions<IntegrationEventSubscriptionInfo> subscriptionOptions,
    ILogger<OutboxProcessor> logger)
    : BackgroundService
{
    private const int DelayMilliseconds = 5000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OutboxProcessor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();

                var messages = await repository.GetUnprocessedMessagesAsync(stoppingToken);
                if (!messages.Any())
                {
                    await Task.Delay(DelayMilliseconds, stoppingToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    try
                    {
                        var integrationEvent = RebuildEvent(message, subscriptionOptions.Value);
                        if (integrationEvent is null)
                        {
                            logger.LogWarning("Invalid event payload {MessageId}", message.Id);
                            repository.MarkAsFailed(message, false);
                            continue;
                        }

                        // TODO: Consider using a message bus instead of direct handler invocation for better decoupling and scalability
                        var eventType = integrationEvent.GetType();
                        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
                            await handler.Handle(integrationEvent);

                        repository.MarkAsProcessed(message);
                        logger.LogInformation("Processed outbox message {MessageId}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing message {MessageId}", message.Id);
                        repository.MarkAsFailed(message);
                    }
                }

                await repository.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // shutdown graceful
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox processing failed");
                await Task.Delay(DelayMilliseconds, stoppingToken);
            }
        }

        logger.LogInformation("OutboxProcessor stopped");
    }

    private IntegrationEvent? RebuildEvent(OutboxMessage message, IntegrationEventSubscriptionInfo options)
    {
        if (!options.EventTypes.TryGetValue(message.PayloadType, out var type))
        {
            logger.LogWarning("Unknown event type {PayloadType}", message.PayloadType);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize(
                message.Payload,
                type,
                options.JsonSerializerOptions
            ) as IntegrationEvent;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Deserialization failed for {MessageId}", message.Id);
            return null;
        }
    }
}