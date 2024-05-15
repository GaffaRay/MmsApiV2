namespace MmsApiV2.MemberAccounts.Models
{
    public class MemberAccountDTO
    {
        /// <summary>
        /// The EGYM account ID for the gym member.
        /// </summary>
        public string? AccountId { get; set;  }

        /// <summary>
        /// The member email address. It should be unique within the gym chain.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The member first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The member last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The Member date of birth.<br/>
        /// Format: yyyy-MM-dd
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// The member gender.<br/>
        /// Please note that NON_BINARY is not correctly supported yet, we send it as FEMALE for the usage of the machines.<br/>
        /// Allowed values: MALE, FEMALE, NON_BINARY
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The user's contact information.
        /// </summary>
        public ContactDTO Contact { get; set; }

        /// <summary>
        /// The membership details.
        /// </summary>
        public MembershipDTO Membership { get; set; }
    }
}
