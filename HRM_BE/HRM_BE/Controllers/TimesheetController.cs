using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.API.Helpers;
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
        private TimesheetHelper _helper;

        public TimesheetController( ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _helper = new TimesheetHelper(_context, _userManager, _mapper);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<TimesheetViewModel>), 200)]
        public async Task<IActionResult> GetTimeSheet(CancellationToken token)
        {
            var currentUser = await _context.Users.FindAsync(User.GetId());

            var timesheets = await _context.Timesheets
                .Where(o => o.UserId == currentUser.Id)
                .OrderBy(o => o.StartDate)
                .ProjectTo<TimesheetViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

            return Ok(timesheets);
        }

        [HttpGet("bymonth")]
        [ProducesResponseType(typeof(List<TimesheetViewModel>), 200)]
        public async Task<IActionResult> GetTimeSheetByMonth(CancellationToken token, DateTime month, string userId)
        {
            User currentUser;
                if (string.IsNullOrEmpty(userId) || userId == "-1")
                currentUser = await _context.Users.FindAsync(User.GetId());
            else
            {
                currentUser = new User()
                {
                    Id = userId
                };
            }

            var timesheets = await _context.Timesheets
                .Where(o => o.UserId == currentUser.Id && (o.StartDate.Month == month.Month || o.EndDate.Month == month.Month))
                .OrderBy(o => o.StartDate)
                .ProjectTo<TimesheetViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

            if(timesheets.Count > 0)
            {
                var startDateOfMonth = new DateTime(month.Year, month.Month, 1);
                var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

                if (timesheets[0].StartDate.Date > startDateOfMonth)
                {
                    var resultStart = _helper.GetTimesheetSittedBetweenFunction(currentUser.Id, timesheets[0].StartDate);
                    if (resultStart != null)
                    {
                        timesheets.Insert(0, _mapper.Map<TimesheetViewModel>(resultStart));
                    }
                }

                if (timesheets[timesheets.Count - 1].EndDate.Date < endDateOfMonth)
                {
                    var resultEnd = _helper.GetTimesheetSittedBetweenFunction(currentUser.Id, timesheets[timesheets.Count - 1].EndDate);
                    if (resultEnd != null)
                    {
                        timesheets.Add(_mapper.Map<TimesheetViewModel>(resultEnd));
                    }
                }

                return Ok(timesheets);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(TimesheetRequestModel), (204))]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestModel model)
        {
            Timesheet result = await _context.Timesheets.FirstOrDefaultAsync(x => 
                                    x.UserId == model.UserId && x.StartDate.Date == model.StartDate.Date && 
                                    x.EndDate.Date == model.EndDate.Date ) ?? null;

            if(result != null)
            {
                result.TotalHour = model.TotalHour;
                result.Status = model.Status;
                result.IsChange = true;
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
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<TimesheetRequestModel>(_model));
            }
        }

        [HttpGet("gettimesheetbydate")]
        [ProducesResponseType(typeof(TimesheetViewModel), 200)]
        public async Task<IActionResult>GetTimesheetByDate(CancellationToken token, string usrId, DateTime dateFrom, DateTime dateTo)
        {
            Timesheet result = await _context.Timesheets.FirstOrDefaultAsync(x =>
                                    x.UserId == usrId && x.StartDate.Date == dateFrom.Date && x.EndDate.Date == dateTo.Date);
            return Ok(_mapper.Map<TimesheetViewModel>(result));
        }

        [HttpPost("task/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddTimesheetTask([FromRoute] int id, [FromBody] List<TaskRequestModel> models)
        {
            var timesheetId = id;   
            var userId = User.GetId();
            List<TaskHour> TaskHours = new List<TaskHour>();

            var timesheet = await _context.Timesheets.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (!_context.Timesheets.Any(o => o.Id == timesheetId))
                return BadRequest("Timesheet not found!");

            foreach (TaskRequestModel TaskItem in models)
            {
                var queriedTimesheetTask = await _context.TimesheetTasks.FirstOrDefaultAsync(x => x.Id == TaskItem.Id && x.TimesheetId == TaskItem.TimesheetId) ?? null;

                if(queriedTimesheetTask == null)
                {
                    TimesheetTask task = new TimesheetTask()
                    {
                        TimesheetId = timesheetId,
                        AccountDomain = await _context.AccountDomains.FirstOrDefaultAsync(t => t.Id == TaskItem.AccountDomainId),
                        Activity = await _context.Activities.FirstOrDefaultAsync(t => t.Id == TaskItem.ActivityId),
                        Task = TaskItem.Task,
                        StartDate = timesheet.StartDate,
                        EndDate = timesheet.EndDate
                    };

                    timesheet.Tasks.Add(task);
                }
                else
                {
                    queriedTimesheetTask.AccountDomain = await _context.AccountDomains.FirstOrDefaultAsync(t => t.Id == TaskItem.AccountDomainId);
                    queriedTimesheetTask.Activity = await _context.Activities.FirstOrDefaultAsync(t => t.Id == TaskItem.ActivityId);
                    queriedTimesheetTask.Task = TaskItem.Task;
                }
            }

            _context.Timesheets.Update(timesheet);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TimesheetViewModel>(timesheet));
        }

        [HttpPost("addhour")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddTaskHours([FromBody] TaskHour taskHours)
        {
            if (taskHours != null)
            {
                var existedTaskHours = await _context.TaskHour.Where(x => x.TimeSheetTaskId == taskHours.TimeSheetTaskId).ToListAsync();

                if (taskHours.Id == -1)
                {
                    taskHours.Id = new Int16();
                    await _context.AddAsync(taskHours);
                    await _context.SaveChangesAsync();
                }

                foreach (var existedHour in existedTaskHours)
                {
                    if (existedHour.Id == taskHours.Id && existedHour.WorkingDate.Date == taskHours.WorkingDate.Date)
                    {
                        existedHour.WorkingHour = taskHours.WorkingHour;
                        break;
                    }
                }

                await _context.SaveChangesAsync();
                existedTaskHours.Add(taskHours);
                return Ok(_mapper.Map<List<TaskHourViewModel>>(existedTaskHours));
            }
            else
            {
                return BadRequest("Nothing has been push here!");
            }
        }

        [HttpGet("getsittedbetween")]
        [ProducesResponseType(typeof(TimesheetRequestModel), 200)]
        public async Task<IActionResult> GetTimesheetSittedBetween(string usrId, DateTime targetDate)
        {
            var targetTick = targetDate.Ticks;
            var userTimesheet = await _context.Timesheets.Where(x => x.UserId == usrId).ToListAsync();

            if(userTimesheet != null)
            {
                foreach(var timesheet in userTimesheet)
                {
                    if(targetTick > timesheet.StartDate.Ticks && targetTick < timesheet.EndDate.Ticks)
                    {
                        return Ok(_mapper.Map<TimesheetRequestModel>(timesheet));
                    }
                }
            }

            return BadRequest("Not found!");
        }

        [HttpGet("getoverview")]
        [ProducesResponseType(typeof(UserTimesheetOverViewViewModel), 200)]
        public async Task<IActionResult> GetTimesheetOverView(string usrId, DateTime month)
        {
            var result = await _helper.UserTimesheetOverView(usrId, month);

            return Ok(result);
        }
    }
}
