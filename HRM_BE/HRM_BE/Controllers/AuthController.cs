﻿using System;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IAuthService authService, UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _userManager = userManager;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {
                    Id = "1",
                    UserName = "hqhoangvuong",
                    Email = model.Email 
                };
                var result = await _userManager.CreateAsync(user, model.Password);
            }

            return Ok();
        }
    }
}
