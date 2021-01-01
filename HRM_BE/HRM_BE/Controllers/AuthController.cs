using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HRM.Core.Constants;
using HRM.Core.Exceptions;
using HRM.Core.Models.Users;
using HRM.Service.Services;
using HRM.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HRM.API.RequestModel;
using HRM.Core.Data;

namespace HRM.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IAuthService authService, UserManager<User> userManager, ApplicationDbContext context,ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.ValidateUserAsync(request.Email, request.Password);
                var securityToken = await _authService.GetJwtTokenAsync(user);

                return Ok(new JwtToken
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    ExpiredTime = securityToken.ValidTo
                });
            } catch (Exception ex) when (!(ex is ModelStateException))
            {
                _logger.LogWarning("Failed to login");
                return StatusCode(500);
            }
        }

        [HttpPost("Register/{accDomainId}")]
        public async Task<IActionResult> Register([FromBody] CreateUserModel newUser, string accDomainId)
        {
            var currentUserId = int.Parse(_userManager.Users.OrderByDescending(p => p.Id).FirstOrDefault().Id);
            if (ModelState.IsValid)
            {
                var user = new User {
                    Id = (currentUserId + 1).ToString(),
                    UserName = newUser.Username,
                    Email = newUser.Email,
                    IsTeamLead = false
                };

                var result = await _userManager.CreateAsync(user, newUser.Password);

                var usrAccDomain = new UserAccountDomain()
                {
                    AccountDomainId = accDomainId,
                    UserId = user.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = new DateTime(2099, 1, 1),
                    IsActive = true
                };

                var job = new Job()
                {
                    Id = (int.Parse(_context.Jobs.OrderByDescending(p => p.Id).FirstOrDefault().Id) + 1).ToString(),
                    UserId = user.Id,
                    JobTitle = "New Employee",
                    Resource = "None"
                };
                
                await _context.UserAccountDomain.AddAsync(usrAccDomain);
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            return BadRequest("invaild user data");
        }
    }
}
