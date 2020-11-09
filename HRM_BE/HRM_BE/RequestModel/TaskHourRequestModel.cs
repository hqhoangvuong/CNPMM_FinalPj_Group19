using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.RequestModel
{
    public class TaskHourRequestModel
    {
        public DateTime WorkingDate { get; set; }
        public decimal WorkingHour { get; set; }
    }
}
