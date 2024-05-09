namespace MmsApiV2.MemberAccounts.Models;

/// <summary>
/// The user's contact information.
/// </summary>
public class ContactDTO
{
    /// <summary>
    /// The member telephone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Street name in member's address.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Street number in member's address.
    /// </summary>
    public string StreetNumber { get; set; }

    /// <summary>
    /// Zip code in member's address.
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// City in member's address.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State in member's address.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Country of the member's address in ISO 3166-1 alpha-2.<br/>
    /// Example: DE
    /// </summary>
    public string Country { get; set; }
}
