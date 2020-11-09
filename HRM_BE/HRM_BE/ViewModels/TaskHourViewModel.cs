using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class TaskHourViewModel
    {
        public string Id { get; set; }
        public DateTime WorkingDate { get; set; }
        public decimal WorkingHour { get; set; }
    }
}
