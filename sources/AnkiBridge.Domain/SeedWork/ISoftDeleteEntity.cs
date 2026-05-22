namespace AnkiBridge.Domain.SeedWork;

public interface ISoftDeleteEntity
{
    /// <summary>
    /// Gets a value indicating whether the entity has been marked as deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the date and time when the entity was deleted.
    /// </summary>
    DateTimeOffset? DeletedAt { get; }

    /// <summary>
    /// Gets the identifier of the user who deleted the entity.
    /// </summary>
    Guid? DeletedBy { get; }
}