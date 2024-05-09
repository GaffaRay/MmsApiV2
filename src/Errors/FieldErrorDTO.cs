namespace MmsApiV2.Errors;

/// <summary>
/// Represents a field error in the Data Transfer Object (DTO) format.
/// </summary>
public class FieldErrorDTO
{
    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the value that was rejected and caused the error.
    /// </summary>
    public object? RejectedValue { get; set; }
}
