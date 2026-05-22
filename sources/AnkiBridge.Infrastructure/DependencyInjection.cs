using AnkiBridge.Application.Abstractions.Services;
using AnkiBridge.Application.Abstractions.Services.Anki;
using AnkiBridge.Application.Abstractions.TransactionalOutbox;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.Dictionary.Abstractions;
using AnkiBridge.Application.Features.Learning.Abstractions;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Interceptors;
using AnkiBridge.Infrastructure.Persistence.QueryServices;
using AnkiBridge.Infrastructure.Persistence.Repositories;
using AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;
using AnkiBridge.Infrastructure.Services.Anki;
using AnkiBridge.Infrastructure.Services.FileStorage;
using AnkiBridge.Infrastructure.TransactionalOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AnkiBridge.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // Add database
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("ankibridgedb"));
        });
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        // Add interceptors
        services.AddScoped<ISaveChangesInterceptor, DomainEventDispatchInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditingInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeletingInterceptor>();

        // Add seeders
        services.AddScoped<IDbSeeder, DictionarySeeder>();
        services.AddScoped<IDbSeeder, LearningSeeder>();
        services.AddScoped<IDbSeeder, AnkiNoteTypeSeeder>();
        services.AddScoped<IDbSeeder, AnkiDeckSeeder>();
        services.AddScoped<IDbSeeder, AnkiNoteSeeder>();

        // Add query services
        services.AddScoped<IDictionaryEntryQueryService, DictionaryEntryQueryService>();
        services.AddScoped<ILearningEntryQueryService, LearningEntryQueryService>();
        services.AddScoped<IAnkiDeckQueryService, AnkiDeckQueryService>();
        services.AddScoped<IAnkiNoteTypeQueryService, AnkiNoteTypeQueryService>();
        services.AddScoped<IAnkiNoteQueryService, AnkiNoteQueryService>();

        // Add repositories
        services.AddScoped<ILearningEntryRepository, LearningEntryRepository>();
        services.AddScoped<IAnkiDeckRepository, AnkiDeckRepository>();
        services.AddScoped<IAnkiNoteTypeRepository, AnkiNoteTypeRepository>();
        services.AddScoped<IAnkiNoteRepository, AnkiNoteRepository>();

        // Add transactional outbox
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddHostedService<TransactionalOutboxService>();

        // Add storage
        builder.AddAzureBlobServiceClient("blobs");
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();

        // Add anki service
        services.AddScoped<IAnkiService, AnkiService>();
        services.AddHttpClient<IAnkiClient, AnkiClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8765"); // AnkiConnect
        });

        return builder;
    }
}
