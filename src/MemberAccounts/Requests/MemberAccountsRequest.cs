using System.Text;

namespace MmsApiV2.MemberAccounts.Requests;

/// <summary>
/// Represents a request for member accounts.
/// </summary>
public class MemberAccountsRequest
{
    /// <summary>
    /// Gets or sets a value indicating whether to only include current gym members.
    /// <br/>
    /// Default: true
    /// </summary>
    public bool CurrentGymOnly { get; set; } = true;

    /// <summary>
    /// Gets or sets the start date and time for the member account request.
    /// </summary>
    public DateTime? FromTimestamp { get; set; }

    /// <summary>
    /// Gets or sets the limit for the number of member accounts to return.<br/>
    /// Default: 10
    /// </summary>
    public int Limit { get; set; } = 10;

    /// <summary>
    /// Gets or sets the offset for the member account request.
    /// <br/>
    /// Default: 0
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Gets or sets the end date and time for the member account request.
    /// </summary>
    public DateTime? ToTimestamp { get; set; }

    /// <summary>
    /// Converts the member account request to query parameters.
    /// </summary>
    /// <returns>A string representing the query parameters.</returns>
    internal string ToQueryParams()
    {
        var queryParams = new StringBuilder();

        queryParams.Append("?currentGymOnly=").Append(CurrentGymOnly.ToString().ToLower());

        if (FromTimestamp.HasValue)
        {
            queryParams.Append("&fromTimestamp=").AppendFormat("{0:o}", FromTimestamp.Value);
        }

        queryParams.Append("&limit=").Append(Limit);
        queryParams.Append("&offset=").Append(Offset);

        if (ToTimestamp.HasValue)
        {
            queryParams.Append("&toTimestamp=").AppendFormat("{0:o}", ToTimestamp.Value);
        }

        return queryParams.ToString();
    }
}
