using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb();
var database = postgres.AddDatabase("lexibridgedb");

//var cache = builder.AddRedis("cache")
//    .WithRedisInsight();

var blobs = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite => azurite.WithLifetime(ContainerLifetime.Persistent))
    .AddBlobs("blobs");

var migrationService = builder.AddProject<LexiBridge_MigrationService>("migrationservice")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<LexiBridge_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitFor(database)
    //.WithReference(cache)
    //.WaitFor(cache)
    .WithReference(blobs)
    .WaitFor(blobs)
    .WaitForCompletion(migrationService);

builder.Build().Run();
