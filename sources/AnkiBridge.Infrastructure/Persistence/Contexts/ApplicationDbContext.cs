using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Contexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<DictionaryEntry> DictionaryEntries { get; private set; }
    internal DbSet<LearningEntry> LearningEntries { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
