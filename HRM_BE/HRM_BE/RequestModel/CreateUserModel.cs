using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.RequestModel
{
    public class CreateUserModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
