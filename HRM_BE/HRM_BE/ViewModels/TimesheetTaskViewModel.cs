using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class TimesheetTaskViewModel
    {
        public string Id { get; set; }
        public string AccountDomainId { get; set; }
        public string AccountDomainName { get; set; }
        public string ActivityId { get; set; }
        public string ActivityDescription { get; set; }
        public string Task { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<TaskHourViewModel> TaskHours { get; set; }
    }
}
