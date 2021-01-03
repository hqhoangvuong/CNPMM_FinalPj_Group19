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

namespace HRM.API.Helpers
{
    public class TimesheetHelper
    {
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public TimesheetHelper(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            this._context = context;
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public Timesheet GetTimesheetSittedBetweenFunction(string usrId, DateTime targetDate)
        {
            var targetTick = targetDate.Date.Ticks;
            List<Timesheet> userTimesheet = _context.Timesheets.Where(x => x.UserId == usrId).ToList();

            if (userTimesheet != null)
            {
                foreach (var timesheet in userTimesheet)
                {
                    if (targetTick > timesheet.StartDate.Date.Ticks && targetTick < timesheet.EndDate.Date.Ticks)
                    {
                        return timesheet;
                    }
                }
            }

            return null;
        }

        private async Task<List<Timesheet>> GetTimeSheetByMonth(int month, string userId)
        {
            var timesheets = await _context.Timesheets
                .Where(o => o.UserId == userId && (o.StartDate.Month == month || o.EndDate.Month == month))
                .OrderBy(o => o.StartDate)
                .ToListAsync();

            DateTime now = DateTime.Now;
            var startDateOfMonth = new DateTime(now.Year, now.Month, 1);
            var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);
            if (timesheets.Count > 0)
            {
                if (timesheets[0].StartDate.Date > startDateOfMonth)
                {
                    var resultStart = GetTimesheetSittedBetweenFunction(userId, timesheets[0].StartDate);
                    if (resultStart != null)
                    {
                        timesheets.Insert(0, resultStart);
                    }
                }

                if (timesheets[timesheets.Count - 1].EndDate.Date < endDateOfMonth)
                {
                    var resultEnd = GetTimesheetSittedBetweenFunction(userId, timesheets[timesheets.Count - 1].EndDate);
                    if (resultEnd != null)
                    {
                        timesheets.Add(resultEnd);
                    }
                }

                return timesheets;
            }
            else
            {
                return null;
            }

        }

        public async Task<UserTimesheetOverViewViewModel> UserTimesheetOverView(string userId, DateTime month)
        {
            List<Timesheet> timesheets = await GetTimeSheetByMonth(month.Month, userId);
            List<int> loggedTaskId = new List<int>();
            UserTimesheetOverViewViewModel timesheetOverView = new UserTimesheetOverViewViewModel() { 
                UserId = userId,
                Month = month.Month
            };

            if (timesheets != null)
            {
                var startDateOfMonth = new DateTime(month.Year, month.Month, 1);
                var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

                var startMonthTick = startDateOfMonth.Date.Ticks;
                var endMonthTick = endDateOfMonth.Date.Ticks;

                foreach (var timesheet in timesheets)
                {
                    if (timesheet.Status == TimeSheetStatus.Submitted)
                    {
                        timesheetOverView.TotalSubmittedTimesheet += 1;
                    }

                    foreach (var task in timesheet.Tasks)
                    {
                        bool isInMonth = false;
                        foreach (var hourLogged in task.TaskHours)
                        {
                            if (hourLogged.WorkingDate.Date.Ticks >= startMonthTick && hourLogged.WorkingDate.Date.Ticks <= endMonthTick)
                            {
                                timesheetOverView.TotalHourLogged += hourLogged.WorkingHour;
                                isInMonth = true;
                            }
                        }

                        if (isInMonth && !loggedTaskId.Contains(task.Id))
                        {
                            loggedTaskId.Add(task.Id);
                            timesheetOverView.TotalTaskLogged += 1;
                        }
                    }
                }
            }
            return timesheetOverView;
        }
    }
}
