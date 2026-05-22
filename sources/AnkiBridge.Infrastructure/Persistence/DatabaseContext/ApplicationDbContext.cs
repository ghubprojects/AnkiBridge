using AnkiBridge.Application.Abstractions.TransactionalOutbox;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using AnkiBridge.Domain.Aggregates.Dictionary;
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
    internal DbSet<AnkiDeck> AnkiDecks { get; private set; }
    internal DbSet<AnkiNoteType> AnkiNoteTypes { get; private set; }
    internal DbSet<AnkiNote> AnkiNotes { get; private set; }
    internal DbSet<OutboxMessage> OutboxMessages { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
