namespace AnkiBridge.Domain.SeedWork;

public interface IAuditableEntity
{
    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    Guid CreatedBy { get; }

    /// <summary>
    /// Gets the date and time when the entity was last modified.
    /// </summary>
    DateTimeOffset? LastModifiedAt { get; }

    /// <summary>
    /// Gets the identifier of the user who last modified the entity.
    /// </summary>
    Guid? LastModifiedBy { get; }
}