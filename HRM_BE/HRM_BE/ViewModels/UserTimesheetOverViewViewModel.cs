using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class UserTimesheetOverViewViewModel
    {
        public string UserId { get; set; }
        public int Month { get; set; }
        public int TotalTaskLogged { get; set; }
        public Decimal TotalHourLogged { get; set; }
        public int TotalSubmittedTimesheet { get; set; }

        public UserTimesheetOverViewViewModel()
        {
            this.UserId = "";
            this.Month = 0;
            this.TotalHourLogged = 0;
            this.TotalTaskLogged = 0;
            this.TotalSubmittedTimesheet = 0;
        }
    }
}
