using System.Collections.Generic;

namespace HRM.Core.Models.Users
{
    public class Team : BaseEntity<string>
    {
        public string Name { get; set; }
        public virtual ICollection<TeamUser> Users { get; set; }
    }
}
