using LexiBridge.Infrastructure.Persistence.Abstractions;
using LexiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LexiBridge.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seeders = scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(x => x.Order);

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync(cancellationToken);

                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                foreach (var seeder in seeders)
                {
                    await seeder.SeedAsync(context, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    context.ChangeTracker.Clear();
                }
                await transaction.CommitAsync(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }
}