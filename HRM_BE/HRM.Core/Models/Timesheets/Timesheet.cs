using HRM.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Timesheets
{
    public enum TimeSheetStatus
    {
        Initial = 0,
        Submitted = 10,
        Approved = 20,
        Rejected = 30,
        UnApproved = 40
    }


    public class Timesheet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalHour { get; set; }

        public TimeSheetStatus Status { get; set; }

        public bool IsChange { get; set; }

        public virtual ICollection<TimesheetTask> Tasks { get; set; }
    }
}
