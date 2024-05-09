namespace MmsApiV2.MemberAccounts.Models;

/// <summary>
/// The membership details.
/// </summary>
public class MembershipDTO
{
    /// <summary>
    /// Member ID of the gym software (must be unique for exactly one member in the gym chain).<br/>
    /// The membershipId should never change, i.e. you cannot use a contract id or similar,<br/>
    /// which would change when the contract is renewed, changed or a new contract is signed.
    /// </summary>
    public string MembershipId { get; set; }

    /// <summary>
    /// The agreement number is the number of the membership between the member and the gym chain.
    /// </summary>
    public string? AgreementNumber { get; set; }

    /// <summary>
    /// Type of the member's membership to determine available features.<br/>
    /// Allowed values: BASIC, PREMIUM, PROSPECT, CORPORATE_FITNESS
    /// </summary>
    public string MembershipType { get; set; }

    /// <summary>
    /// Extension of membershipStatus for further customization for this user. One membershipStatus can have multiple membershipSubType.
    /// </summary>
    public string? MembershipSubType { get; set; }

    /// <summary>
    /// Mandatory when membershipType is 'BASIC' or 'PREMIUM'. Date when the member's contract ends.<br/>
    /// For gyms with automatic contract renewal, send the date when the contract would run out, if cancelled.<br/>
    /// Update the field when the contract was renewed (manually and automatically renewals).<br/>
    /// Format: yyyy-MM-dd
    /// </summary>
    public string? EndOfContract { get; set; }

    /// <summary>
    /// The date when the membership started.<br/>
    /// Format: yyyy-MM-dd
    /// </summary>
    public string StartOfContract { get; set; }

    /// <summary>
    /// MembershipId of the member who referred this member.
    /// </summary>
    public string? ReferringMemberId { get; set; }

    /// <summary>
    /// Member's barcode to check-in in the gym.<br/>
    /// Can be visualised on the Branded Member App and used for lookup to verify membership.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Mandatory when creating new membership of type 'CORPORATE_FITNESS' or when updating membership to 'CORPORATE_FITNESS'.<br/>
    /// Member's verification TAN can be visualised on the Qualitrain Member App and used for Qualitrain Membership verification.
    /// </summary>
    public string? VerificationTAN { get; set; }

    /// <summary>
    /// The corporate fitness details. These fields are returned only in case membershipType=CORPORATE_FITNESS
    /// </summary>
    public CorporateFitnessDTO? CorporateFitness { get; set; }
}
