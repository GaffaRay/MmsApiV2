using System.Text;

namespace MmsApiV2.TrainerTasks.Requests;

/// <summary>
/// Represents a request for tasks.
/// </summary>
public class TasksRequest
{
    /// <summary>
    /// Gets or sets the date and time when the tasks were last modified.
    /// </summary>
    public string? ModifiedSince { get; set; }

    /// <summary>
    /// Converts the request parameters into a query string.
    /// </summary>
    /// <returns>A query string representing the request parameters.</returns>
    internal string ToQueryParams()
    {
        var queryParams = new StringBuilder();

        if (ModifiedSince != null)
        {
            queryParams.Append("?modifiedSince=").Append(ModifiedSince);
        }

        return queryParams.ToString();
    }
}
