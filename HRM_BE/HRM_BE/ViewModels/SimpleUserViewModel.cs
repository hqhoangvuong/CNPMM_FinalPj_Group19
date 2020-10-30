using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM_BE.ViewModels
{
    public class SimpleUserViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ? $" {FirstName} {LastName}" : Email;
    }
}
