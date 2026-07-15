using AnkiBridge.Application.Abstractions.Dictionary;
using AnkiBridge.Application.Abstractions.Images;
using AnkiBridge.Application.Abstractions.Speech;
using AnkiBridge.Application.Abstractions.Storage;
using AnkiBridge.Application.Abstractions.Translation;
using AnkiBridge.Application.Features.Flashcard.Contracts.Anki;
using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.Contexts;
using AnkiBridge.Infrastructure.Persistence.Repositories;
using AnkiBridge.Infrastructure.Services.AnkiConnect;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;
using AnkiBridge.Infrastructure.Services.Images;
using AnkiBridge.Infrastructure.Services.Images.Pexels;
using AnkiBridge.Infrastructure.Services.Images.Pixabay;
using AnkiBridge.Infrastructure.Services.Speech.Google;
using AnkiBridge.Infrastructure.Services.Storage.AzureBlob;
using AnkiBridge.Infrastructure.Services.Translation;
using AnkiBridge.Infrastructure.Services.Translation.Google;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace AnkiBridge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("Default")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? configuration.GetConnectionString("ankibridge")
            ?? throw new InvalidOperationException("Connection string 'Default' is required.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseConnectionString));

        services.AddScoped<IDictionaryEntryRepository, DictionaryEntryRepository>();
        services.AddScoped<ILearningEntryRepository, LearningEntryRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.Configure<CambridgeDictionaryOptions>(
            configuration.GetSection(CambridgeDictionaryOptions.SectionName));
        services.Configure<GoogleTranslateOptions>(
            configuration.GetSection(GoogleTranslateOptions.SectionName));
        services.Configure<PexelsOptions>(
            configuration.GetSection(PexelsOptions.SectionName));
        services.Configure<PixabayOptions>(
            configuration.GetSection(PixabayOptions.SectionName));
        services.Configure<GoogleSpeechOptions>(
            configuration.GetSection(GoogleSpeechOptions.SectionName));

        services.AddHttpClient<IAnkiConnectClient, AnkiConnectClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["AnkiConnect:BaseUrl"] ?? "http://127.0.0.1:8765/");
        });
        services.AddScoped<IAnkiService, AnkiConnectService>();

        services.AddScoped<CambridgeDictionaryEntryProvider>();
        services.AddScoped<IDictionaryEntryProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<CambridgeDictionaryEntryProvider>());
        services.AddScoped<CambridgeDictionaryPhraseIpaProvider>();
        services.AddScoped<IPhraseIpaProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<CambridgeDictionaryPhraseIpaProvider>());

        services.AddHttpClient<GoogleTranslationProvider>();
        services.AddScoped<CambridgeDictionaryTranslationProvider>();
        services.AddScoped<FallbackTranslationProvider>();
        services.AddScoped<ITranslationProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<FallbackTranslationProvider>());

        services.AddHttpClient<PixabayImageProvider>();
        services.AddHttpClient<PexelsImageProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PexelsOptions>>().Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
        });
        services.AddScoped<FallbackImageProvider>();
        services.AddScoped<IImageProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<FallbackImageProvider>());

        services.AddScoped<ISpeechSynthesisProvider, GoogleSpeechSynthesisProvider>();

        var blobConnectionString = configuration["Azurite:ConnectionString"]
            ?? configuration.GetConnectionString("AzureBlobStorage")
            ?? "UseDevelopmentStorage=true";
        services.AddSingleton(new BlobServiceClient(blobConnectionString));
        services.AddScoped<IFileStorage, AzureBlobStorage>();

        return services;
    }
}
