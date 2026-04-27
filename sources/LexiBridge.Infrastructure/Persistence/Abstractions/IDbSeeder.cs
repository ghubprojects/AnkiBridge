using LexiBridge.Infrastructure.Persistence.Contexts;

namespace LexiBridge.Infrastructure.Persistence.Abstractions;

public interface IDbSeeder
{
    int Order { get; }
    Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken);
}