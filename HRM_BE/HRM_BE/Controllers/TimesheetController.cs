using System;
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

        [HttpPost("add")]
        [ProducesResponseType(typeof(TimesheetRequestModel), (200))]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestModel model)
        {


            var result = await _context.Timesheets.FirstOrDefaultAsync(x => 
                                    x.UserId == model.UserId && x.StartDate == model.StartDate && x.EndDate == model.EndDate ) ?? null;

            if(result != null)
            {
                result.TotalHour = model.TotalHour;
                result.Status = model.Status;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<TimesheetRequestModel>(result));

            }
            else
            {
                Timesheet _model = new Timesheet()
                {
                    UserId = User.GetId(),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TotalHour = model.TotalHour,
                    Status = model.Status
                };
                await _context.Timesheets.AddAsync(_model);
                return Ok(_mapper.Map<TimesheetRequestModel>(_model));
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimesheet([FromRoute] int id, [FromBody] TimesheetRequestModel timesheet)
        {
            var savedSheet = await _context.Timesheets.FirstOrDefaultAsync(t => t.Id == id);

            if (savedSheet == null)
                return BadRequest();
            else
            {
                savedSheet.StartDate = timesheet.StartDate;
                savedSheet.EndDate = timesheet.EndDate;
                savedSheet.TotalHour = timesheet.TotalHour;
                savedSheet.Status = timesheet.Status;

                foreach(var task in savedSheet.Tasks)
                {
                    _context.TimesheetTasks.Remove(task);
                }

                savedSheet.Tasks = new List<TimesheetTask>();

                foreach(var task in timesheet.Tasks)
                {
                    TimesheetTask _task = new TimesheetTask()
                    {
                        TimesheetId = savedSheet.Id,
                        AccountDomain = await _context.AccountDomains.FirstOrDefaultAsync(t => t.Id == task.AccountDomainId),
                        Activity = await _context.Activities.FirstOrDefaultAsync(t => t.Id == task.ActivityId),
                        Task = task.Task,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        TaskHours = new List<TaskHour>()
                    };

                    foreach(TaskHourRequestModel hour in task.TaskHours)
                    {
                        TaskHour _hour = new TaskHour()
                        {
                            WorkingDate = hour.WorkingDate,
                            WorkingHour = hour.WorkingHour
                        };

                        _task.TaskHours.Add(_hour);
                    }

                    savedSheet.Tasks.Add(_task);
                }

                _context.Timesheets.Update(savedSheet);
                await _context.SaveChangesAsync();
            }
            
            return Ok(_mapper.Map<TimesheetViewModel>(savedSheet));
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
