using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class DirectoryViewModel
    {
        public Byte[] Image { get; set; }
        public string FullName { get; set; }
        public string VietnameseName { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeId { get; set; }
        public string Project { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string SeatingPlan { get; set; }
    }
}
