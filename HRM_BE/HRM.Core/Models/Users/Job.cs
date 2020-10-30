using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Users
{
    public class Job : BaseEntity<String>
    {
        public string UserId { get; set; }
        public string JobTitle { get; set; }
        public string Resource { get; set; }
        public string Department { get; set; }
        public string WorkingOffice { get; set; }
        public string SeatingPlan { get; set; }
        public string WorkLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }

    }
}
