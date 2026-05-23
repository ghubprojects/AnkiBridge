using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace AnkiBridge.Application.Common.IntegrationEvents;

public class IntegrationEventSubscriptionInfo
{
    public Dictionary<string, Type> EventTypes { get; } = [];

    public JsonSerializerOptions JsonSerializerOptions { get; } = new(DefaultSerializerOptions);

    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault 
            ? new DefaultJsonTypeInfoResolver() 
            : JsonTypeInfoResolver.Combine()
    };
}
