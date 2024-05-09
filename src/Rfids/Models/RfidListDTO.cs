namespace MmsApiV2.Rfids.Models;

/// <summary>
/// Data Transfer Object for the list of RFIDs.
/// </summary>
public class RfidListDTO
{
    /// <summary>
    /// List of the gym member's RFIDs.
    /// </summary>
    public IEnumerable<RfidDTO> Rfids { get; set; }
}
