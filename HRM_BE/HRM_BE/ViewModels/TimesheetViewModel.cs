using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class TimesheetViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalHour { get; set; }

        public int Status { get; set; }

        public IEnumerable<TimesheetTaskViewModel> Tasks { get; set; }
    }
}
