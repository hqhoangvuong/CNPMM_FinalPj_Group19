using HRM.Core.Models.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.RequestModel
{
    public class TimesheetRequestModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalHour { get; set; }
        public TimeSheetStatus Status { get; set; }
    }
}
