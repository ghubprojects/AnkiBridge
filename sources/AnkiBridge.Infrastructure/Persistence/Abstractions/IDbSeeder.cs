using AnkiBridge.Infrastructure.Persistence.DatabaseContext;

namespace AnkiBridge.Infrastructure.Persistence.Abstractions;

public interface IDbSeeder
{
    int Order { get; }
    Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken);
}