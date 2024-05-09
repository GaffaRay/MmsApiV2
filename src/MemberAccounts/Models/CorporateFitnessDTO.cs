namespace MmsApiV2.MemberAccounts.Models;

/// <summary>
/// The corporate fitness details. These fields are returned only in case membershipType=CORPORATE_FITNESS
/// </summary>
public class CorporateFitnessDTO
{
    /// <summary>
    /// The date (in ISO format) when the corporate fitness (Qualitrain/Wellpass) membership will start, in case that is in the future.<br/>
    /// This field will not be returned in case of an active or past corporate fitness membership.<br/>
    /// Example: 2021-03-08T18:26:47Z
    /// </summary>
    public DateTime? StartTimestamp { get; set; }

    /// <summary>
    /// The date (in ISO format) when the corporate fitness (Qualitrain/Wellpass) membership ends.<br/>
    /// This field is only returned in case there is a defined end timestamp for the corporate fitness membership.<br/>
    /// Example: 2021-03-08T18:26:47Z
    /// </summary>
    public DateTime? EndTimestamp { get; set; }
}
