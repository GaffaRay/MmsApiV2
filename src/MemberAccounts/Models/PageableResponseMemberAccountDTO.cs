namespace MmsApiV2.MemberAccounts.Models;

/// <summary>
/// Represents a pageable response for member account data transfer objects.
/// </summary>
public class PageableResponseMemberAccountDTO
{
    /// <summary>
    /// Gets or sets the offset for the pagination.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Gets or sets the limit for the pagination.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// Gets or sets the items for the current page.
    /// </summary>
    public IEnumerable<MemberAccountDTO> Items { get; set; }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNext { get; set; }
}
