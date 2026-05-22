namespace AnkiBridge.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Asynchronously saves all changes made in this context to the underlying database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}