namespace MmsApiV2.Errors;

/// <summary>
/// Represents an error data transfer object.
/// </summary>
public class ErrorDTO
{
    /// <summary>
    /// Gets or sets the timestamp of the request.
    /// </summary>
    public string? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the path requested.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the request id associated with the error.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets or sets the http status code.
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the message that descripes the error.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the list of field errors.
    /// </summary>
    public List<FieldErrorDTO>? FieldErrors { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the error.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
}
