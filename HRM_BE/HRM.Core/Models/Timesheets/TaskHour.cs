using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Timesheets
{
    public class TaskHour
    {
        public int Id { get; set; }
        public int TimeSheetTaskId { get; set; }
        public virtual TimesheetTask Task { get; set; }
        public DateTime WorkingDate { get; set; }
        public decimal WorkingHour { get; set; } = 0;
    }
}
