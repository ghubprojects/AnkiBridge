using LexiBridge.Domain.Aggregates.Dictionary;
using LexiBridge.Domain.Aggregates.Exporting.Decks;
using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LexiBridge.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<DictionaryEntry> DictionaryEntries { get; private set; }
    internal DbSet<LearningItem> LearningItems { get; private set; }
    internal DbSet<LearningItemExport> LearningItemExports { get; private set; }
    internal DbSet<Deck> Decks { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
