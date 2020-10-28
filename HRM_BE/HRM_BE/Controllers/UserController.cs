﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HRM.Core.Data;
using HRM.Core.Models.Users;
using AutoMapper;
using System.Threading;
using HRM.Core.Extensions;
using HRM.API.ViewModels;

namespace HRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("current")]
        [ProducesResponseType(typeof(UserViewModel), statusCode:200)]
        public async Task<IActionResult> GetCurrentUser(CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(User.GetId());
            return Ok(_mapper.Map<UserViewModel>(user));
        }
    }
}