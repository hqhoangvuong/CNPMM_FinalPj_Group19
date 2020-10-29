using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HRM.Core.Models.Users
{
    public class User : IdentityUser<string>
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string GoogleToken { get; set; }
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
        public virtual ICollection<UserAccountDomain> UserAccountDomains { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    }
}
