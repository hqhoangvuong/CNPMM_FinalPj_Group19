using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.API.Helpers;
using HRM.API.ViewModels;
using HRM.Core.Data;
using HRM.Core.Extensions;
using HRM.Core.Models.Paging;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRM.Core.Models.Timesheets;

namespace HRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountDomainController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountDomainController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(List<AccountDomain>), 200)]
        public async Task<IActionResult> GetAllAccountDomain(CancellationToken token)
        {
            List<AccountDomain> accountDomain = await _context.AccountDomains.ToListAsync();
            return Ok(accountDomain);
        }

        [HttpGet("getcurrent")]
        [ProducesResponseType(typeof(AccountDomain), 200)]
        public async Task<IActionResult> GetCurrentAccountDomain(CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(User.GetId());
            var currentAccDomainId = user.UserAccountDomains.SingleOrDefault(x => x.IsActive == true);
            var currentAccountDomain = await _context.AccountDomains.SingleOrDefaultAsync(x => x.Id == currentAccDomainId.AccountDomainId);
            return Ok(currentAccountDomain);
        }

        [HttpGet("gettask")]
        [ProducesResponseType(typeof(List<Activity>), 200)]
        public async Task<IActionResult> GetTaskOfAccDomain(CancellationToken token, string accDomainId)
        {
            var tasks = await _context.Activities.Where(x => x.AccountDomainId == accDomainId).ToListAsync();
            return Ok(tasks);
        }

    }
}
