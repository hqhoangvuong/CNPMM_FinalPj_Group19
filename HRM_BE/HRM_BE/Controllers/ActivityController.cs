using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HRM.Core.Data;
using HRM.Core.Extensions;
using HRM.Core.Models.Timesheets;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRM.API.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;


        public ActivityController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpGet]
        [Route("getactivity/{accDomainId}")]
        [ProducesResponseType(typeof(List<Activity>), statusCode: 200)]
        public async Task<IActionResult> GetAllActivityOfAccDomain(CancellationToken token, string accDomainId)
        {
            var activities = await _context.Activities.Where(x => x.AccountDomainId == accDomainId).ToListAsync();
            return Ok(activities);
        }

        [HttpPost]
        [Route("addactivity")]
        [ProducesResponseType(typeof(Activity), statusCode: 204)]
        public async Task<IActionResult> AddActivity([FromBody] Activity activity)
        {
            var currentId = await _context.Activities.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            activity.Id = (int.Parse(currentId.Id) + 1).ToString();

            await _context.AddAsync(activity);
            await _context.SaveChangesAsync();
            return Ok(activity);
        }

        [HttpPost]
        [Route("updateactivity")]
        [ProducesResponseType(typeof(Activity), statusCode: 204)]
        public async Task<IActionResult> Update([FromBody] Activity activity)
        {
            var current = await _context.Activities.FirstOrDefaultAsync(p => p.Id == activity.Id && p.AccountDomainId == activity.AccountDomainId);
            if(current != null)
            {
                current.ActivityDescription = activity.ActivityDescription;
                await _context.SaveChangesAsync();
                return Ok(activity);
            }
            else
            {
                return Ok(new Activity());
            }
        }

        [HttpPost]
        [Route("deleteactivity")]
        [ProducesResponseType(typeof(Activity), statusCode: 204)]
        public async Task<IActionResult> Delete([FromBody] Activity activity)
        {
            var current = await _context.Activities.FirstOrDefaultAsync(p => p.Id == activity.Id && p.AccountDomainId == activity.AccountDomainId);
            if (current != null)
            {
                _context.Activities.Remove(current);
                await _context.SaveChangesAsync();
                return Ok(current);
            }
            else
            {
                return Ok(new Activity());
            }
        }
    }
}
