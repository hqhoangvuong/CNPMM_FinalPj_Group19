using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Timesheets
{
    public class Activity : BaseEntity<string>
    {
        public string AccountDomainId { get; set; }
        public string ActivityDescription { get; set; }
    }
}
