using HRM.Core.Models.Users;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Timesheets
{
    public class TimesheetTask
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public virtual Timesheet Timesheet { get; set; }
        public virtual AccountDomain AccountDomain {get;set;}
        public virtual Activity Activity { get; set; }
        public string Task { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsNew { get; set; } = true;
        public bool IsEdit { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public bool IsHoliday { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<TaskHour> TaskHours { get; set; }
    }
}
