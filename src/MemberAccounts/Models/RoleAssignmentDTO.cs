namespace MmsApiV2.MemberAccounts.Models;

/// <summary>
/// Represents the role assignment data transfer object.
/// </summary>
public class RoleAssignmentDTO
{
    /// <summary>
    /// If trainer is true, assign trainer right for account, otherwise remove trainer rights.
    /// </summary>
    public bool Trainer { get; set; }
}
