using HRM.Core.Models.Users;
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
        public string MiddleName { get; set; }
        public string Name => !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ? $" {FirstName} {LastName}" : Email;
        public bool Gender;
        public string Sex => Gender ? $"Male" : $"Female"; 
        public DateTime DoB { get; set; }
        public string EmployeeCode { get; set; }
        public string PhoneNumber { get; set; }
        public string VietnameseName { get; set; }
        public string EthnicRace { get; set; }
        public string IdCardNo { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string BirthplaceCity { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public bool IsTeamLead { get; set; }
        public IList<TeamViewModel> Teams { get; set; }
        public IList<ProjectViewModel> Projects { get; set; }
        public IList<Job> Jobs { get; set; }
        public IList<RoleViewModel> Roles { get; set; }
        public bool isDeleted { get; set; }
    }
}
