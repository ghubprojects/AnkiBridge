using LexiBridge.Application.Abstractions.Services;
using LexiBridge.Application.Features.Dictionary.Abstractions;
using LexiBridge.Application.Features.Learning.Abstractions;
using LexiBridge.Domain.Aggregates.Exporting.Decks;
using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Infrastructure.Persistence.Abstractions;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.Infrastructure.Persistence.Interceptors;
using LexiBridge.Infrastructure.Persistence.QueryServices;
using LexiBridge.Infrastructure.Persistence.Repositories;
using LexiBridge.Infrastructure.Persistence.Seeding.Seeders;
using LexiBridge.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LexiBridge.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // Add database
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options
                .AddInterceptors(serviceProvider.GetRequiredService<ISaveChangesInterceptor>())
                .UseNpgsql(configuration.GetConnectionString("lexibridgedb"));
        });
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        // Add interceptors
        services.AddScoped<ISaveChangesInterceptor, AuditingInterceptor>();

        // Add seeders
        services.AddScoped<IDbSeeder, LearningSeeder>();
        services.AddScoped<IDbSeeder, DictionarySeeder>();

        // Add query services
        services.AddScoped<IDictionaryEntryQueryService, DictionaryEntryQueryService>();
        services.AddScoped<ILearningItemQueryService, LearningItemQueryService>();

        // Add repositories
        services.AddScoped<ILearningItemRepository, LearningItemRepository>();
        services.AddScoped<ILearningItemExportRepository, LearningItemExportRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();

        // Add storage
        builder.AddAzureBlobServiceClient("blobs");
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();

        return builder;
    }
}
