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

namespace HRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectoryController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(DirectoryViewModel), statusCode: 200)]
        public async Task<IActionResult> GetDirectoryPaging(
            [FromQuery] PagingDto pagingDto,
            CancellationToken token)
        {
            var query = await QueryUser();
            var total = await query.CountAsync(token);
            var directories = await query
                .Skip((pagingDto.Page - 1) * pagingDto.PageSize)
                .Take(pagingDto.PageSize)
                .ToListAsync(token);

            var itemLits = new List<DirectoryViewModel>();
            foreach(var item in directories)
            {
                var currentJob = item.Jobs.FirstOrDefault(t => t.IsActive = true);

                var viewModel = new DirectoryViewModel()
                {
                    FullName = string.Format("{0} {1}", item.FirstName, item.LastName),
                    JobTitle = currentJob.JobTitle,
                    EmployeeId = item.EmployeeCode,
                    Project = item.UserAccountDomains.FirstOrDefault(t => t.IsActive = true).AccountDomain.Name,
                    Department = currentJob.Resource,
                    Phone = item.PhoneNumber,
                    Email = item.Email,
                    SeatingPlan = currentJob.SeatingPlan
                };

                if (item.IsHasAvatar)
                {
                     viewModel.Image = System.IO.File.ReadAllBytes(ImageProcessing.savePath + "/avatar_" + item.Id + ".image"); 
                }

                itemLits.Add(viewModel);
            }

            var pagingList = new PagingList<DirectoryViewModel>
            {
                Paging = new Paging
                {
                    Filter = pagingDto.Filter,
                    Page = pagingDto.Page,
                    PageSize = pagingDto.PageSize,
                    Total = total
                },
                Items = itemLits
            };
            return Ok(pagingList);
        }

        private async Task<IQueryable<User>> QueryUser()
        {
            var query = _context.Users
                .AsQueryable();

            query = query
                .OrderByDescending(tt => tt.EmployeeCode);
            return query;
        }
    }
}
