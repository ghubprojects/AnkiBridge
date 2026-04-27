using LexiBridge.Application;
using LexiBridge.Infrastructure;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.MigrationService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructureServices();

builder.Services.AddHostedService<Worker>();

//builder.AddNpgsqlDbContext<ApplicationDbContext>("lexibridgedb", configureDbContextOptions: optionBuilder =>
//{
//    optionBuilder
//        .UseNpgsql(builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
//});

var host = builder.Build();
host.Run();
