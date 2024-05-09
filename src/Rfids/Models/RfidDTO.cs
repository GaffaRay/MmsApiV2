namespace MmsApiV2.Rfids.Models;

/// <summary>
/// This class represents the Data Transfer Object for the RFID data.
/// </summary>
public class RfidDTO
{
    /// <summary>
    /// String representing rfid value in hex (with leading "0x").<br/>
    /// Please ensure that only RFID information is sent in this field but no other medium like magnetic stripe card numbers or bar codes.<br/>
    /// Match pattern: ^0[x][0-9a-fA-F]+$
    /// </summary>
    public string Rfid { get; set; }

    /// <summary>
    /// The rfid tag format.<br/>
    /// Allowed values: HITAG1, MIFARE, LEGIC
    /// </summary>
    public string TagFormat { get; set; }
}
