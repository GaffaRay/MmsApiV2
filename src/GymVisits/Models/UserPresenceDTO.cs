namespace MmsApiV2.GymVisits.Models;

/// <summary>
/// Represents the data transfer object for user presence.
/// </summary>
public class UserPresenceDTO
{
    /// <summary>
    /// Timestamp in milliseconds since epoch for a checkin / checkout event that happened in the past.<br/>
    /// Between -9007199254740991 and 9007199254740991
    /// </summary>
    public long? Timestamp { get; set; }
}
