namespace MmsApiV2.Errors;

/// <summary>
/// Represents exceptions that occur during MmsApiV2 operations.
/// This class cannot be inherited.
/// </summary>
public class MmsApiV2Exception : Exception
{
    /// <summary>
    /// Gets or sets the http code.
    /// </summary>
    public int ReturnCode { get; set; }

    /// <summary>
    /// Gets or sets the error details of the exception.
    /// </summary>
    public ErrorDTO? Error { get; set; }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class with a specified return code, error details, and a message that describes the error.
    /// </summary>
    public MmsApiV2Exception(int returnCode, ErrorDTO? error, string? message) : base(message)
    {
        ReturnCode = returnCode;
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class with a specified return code and a message that describes the error.
    /// </summary>
    public MmsApiV2Exception(int returnCode, string? message) : base(message)
    {
        ReturnCode = returnCode;
    }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class with a specified return code, a message that describes the error, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public MmsApiV2Exception(int returnCode, string? message, Exception? innerException) : base(message, innerException)
    {
        ReturnCode = returnCode;
    }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class.
    /// </summary>
    public MmsApiV2Exception() { }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class with a message that describes the error.
    /// </summary>
    public MmsApiV2Exception(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the MmsApiV2Exception class with a message that describes the error and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public MmsApiV2Exception(string? message, Exception? innerException) : base(message, innerException) { }
}
