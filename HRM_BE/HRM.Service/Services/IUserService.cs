using HRM.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(User entity, string password = null);
        Task<User> ValidateUserAsync(string email, string password);
        Task UpdatePasswordAsync(User entity, string password);
    }
}
