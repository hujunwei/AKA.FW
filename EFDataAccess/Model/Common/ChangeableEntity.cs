namespace EFDataAccess.Model.Common;

public class ChangeableEntity<TEntityId> : IRowVersionedEntity
{
    public TEntityId Id { get; set; } = default!;

    public string CreatedBy { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = default!;

    public string UpdatedBy { get; set; } = default!;

    public DateTime UpdatedAt { get; set; } = default!;

    /// <summary>
    /// This field will be used in optimistic concurrency handling.
    /// DO NOT read/write its value outside data accessor.
    /// </summary>
    public byte[] _RowVersion { get; set; } = default!;
}