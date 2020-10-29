using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ? $" {FirstName} {LastName}" : Email; 
        public string EmployeeCode { get; set; }
        public string PhoneNumber { get; set; }
        public IList<TeamViewModel> Teams { get; set; }
        public IList<ProjectViewModel> Projects { get; set; }
        public IList<RoleViewModel> Roles { get; set; }
        public bool isDeleted { get; set; }
    }
}
