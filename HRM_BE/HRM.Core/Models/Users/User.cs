using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HRM.Core.Models.Users
{
    public class User : IdentityUser<string>
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        public bool Gender { get; set; }
        public string VietnameseName { get; set; }
        public string EthnicRace { get; set; }
        public string IdCardNo { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string BirthplaceCity { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public string GoogleToken { get; set; }
        public bool IsHasAvatar { get; set; }
        public bool IsTeamLead { get; set; }
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
        public virtual ICollection<UserAccountDomain> UserAccountDomains { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
