using AnkiBridge.Application.Common.Contracts.Outbox;
using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Domain.Aggregates.Flashcard.Decks;
using AnkiBridge.Domain.Aggregates.Flashcard.Notes;
using AnkiBridge.Domain.Aggregates.Flashcard.NoteTypes;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AnkiBridge.Infrastructure.Persistence.DatabaseContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<DictionaryEntry> DictionaryEntries { get; private set; }
    internal DbSet<LearningEntry> LearningEntries { get; private set; }
    internal DbSet<Deck> Decks { get; private set; }
    internal DbSet<NoteType> NoteTypes { get; private set; }
    internal DbSet<Note> Notes { get; private set; }
    internal DbSet<OutboxMessage> OutboxMessages { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
