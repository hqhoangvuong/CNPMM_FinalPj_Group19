﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.API.RequestModel;
using HRM.API.ViewModels;
using HRM.Core.Data;
using HRM.Core.Extensions;
using HRM.Core.Models.Timesheets;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public TimesheetController( ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<TimesheetViewModel>), 200)]
        public async Task<IActionResult> GetTimeSheet(CancellationToken token)
        {
            var currentUser = await _context.Users.FindAsync(User.GetId());

            var timesheets = await _context.Timesheets
                .Where(o => o.UserId == currentUser.Id)
                .ProjectTo<TimesheetViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

            return Ok(timesheets);
        }

        [HttpPost()]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestModel model)
        {
            var isExisted = _context.Timesheets.Any(o => o.Id == model.Id);
            if (model.Id != null && !isExisted)
            {
                // int currentId = Convert.ToInt32(_context.Timesheets.Max(t => t.Id)) + 1;
                Timesheet _model = new Timesheet()
                {
                    UserId = User.GetId(),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TotalHour = model.TotalHour,
                    Status = model.Status
                };

                _context.Timesheets.Add(_model);
                await _context.SaveChangesAsync();
                return Ok(_model);
            }

            return BadRequest("Timesheet already existed");
        }

        [HttpPost("task/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddTimesheetTask([FromRoute] int id, [FromBody] List<TaskRequestModel> models)
        {
            var TimesheetId = id;
            List<TaskHour> TaskHours = new List<TaskHour>();

            var timesheet = await _context.Timesheets.FirstOrDefaultAsync(o => o.Id == id);
            if (!_context.Timesheets.Any(o => o.Id == TimesheetId))
                return BadRequest("Timesheet not found!");

            foreach (TaskRequestModel TaskItem in models)
            {

                TimesheetTask task = new TimesheetTask()
                {
                    TimesheetId = TimesheetId,
                    AccountDomain = await _context.AccountDomains.FirstOrDefaultAsync(t => t.Id == TaskItem.AccountDomainId),
                    Activity = await _context.Activities.FirstOrDefaultAsync(t => t.Id == TaskItem.ActivityId),
                    Task = TaskItem.Task,
                    StartDate = TaskItem.StartDate,
                    EndDate = TaskItem.EndDate,
                    TaskHours = new List<TaskHour>()
                };

                foreach (TaskHourRequestModel hour in TaskItem.TaskHours)
                {
                    TaskHour _hour = new TaskHour()
                    {
                        WorkingDate = hour.WorkingDate,
                        WorkingHour = hour.WorkingHour
                    };

                    task.TaskHours.Add(_hour);
                }

                timesheet.Tasks.Add(task);
            }

            _context.Timesheets.Update(timesheet);
            await _context.SaveChangesAsync();

            return Ok(timesheet);
        }
    }
}