using HRM.Core.Models.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.RequestModel
{
    public class TaskRequestModel
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public string AccountDomainId { get; set; }
        public string ActivityId { get; set; }
        public string Task { get; set; }
        public bool IsHoliday { get; set; }
    }
}