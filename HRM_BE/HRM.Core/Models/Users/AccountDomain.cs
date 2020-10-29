using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Core.Models.Users
{
    public class AccountDomain : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Client { get; set; }
        public virtual ICollection<UserAccountDomain> Users { get; set; }
    }
}
