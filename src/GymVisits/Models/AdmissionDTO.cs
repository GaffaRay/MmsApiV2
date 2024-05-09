namespace MmsApiV2.GymVisits.Models;

/// <summary>
/// Represents the data transfer object for Admission.
/// </summary>
public class AdmissionDTO
{
    /// <summary>
    /// If this member has been admitted for the first time in the gym this will be true, false otherwise.
    /// </summary>
    public bool FirstAdmission { get; set; }
}
