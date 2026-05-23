using System.Reflection;
using System.Text.Json;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;

internal static class SeedDataLoader
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public static List<T> GetSeedDataFromResource<T>()
    {
        var resourceDirectoryName = $"AnkiBridge.Infrastructure.Persistence.Seeding.Resources";

        var typeName = typeof(T).Name;
        if (typeName.EndsWith("Seed"))
            typeName = typeName[..^"Seed".Length];

        var resourceName = $"{resourceDirectoryName}.{typeName}.json";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource not found: {resourceName}");

        using var reader = new StreamReader(stream);

        return JsonSerializer.Deserialize<List<T>>(reader.ReadToEnd(), jsonOptions) ?? [];
    }
}