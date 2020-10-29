using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Core.Models.Users
{
    public class UserAccountDomain
    {
        public string AccountDomainId { get; set; }
        public string UserId { get; set; }
        public virtual AccountDomain AccountDomain { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
