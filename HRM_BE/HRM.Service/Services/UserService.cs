using HRM.Core.Exceptions;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<User> CreateAsync(User entity, string password = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePasswordAsync(User entity, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            var entity = await _userManager.FindByEmailAsync(email);
            if (entity == null || !await _userManager.CheckPasswordAsync(entity, password))
                throw new ModelStateException(nameof(password), "Invalid username or password");

            return entity;
        }
    }
}
