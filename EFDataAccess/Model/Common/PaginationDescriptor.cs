namespace EFDataAccess.Model.Common;

/// <summary>
/// PaginationDescriptor models the pagination related fields in a single class.
/// </summary>
public record PaginationDescriptor
{
    public bool RetrieveTotalItemCount { get; set; }

    public int Skip { get; init; }

    public int Take { get; init; }

    public string OrderBy { get; init; } = default!;

    public bool Ascending { get; init; }
}