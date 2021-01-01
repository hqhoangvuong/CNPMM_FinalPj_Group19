using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HRM.Core.Data;
using HRM.Core.Models.Users;
using HRM.Core.Extensions;
using HRM.API.ViewModels;
using HRM_BE.ViewModels;
using HRM.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;

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
        private readonly IImageWriter _imageWriter;

        public UserController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, IImageWriter imageWriter)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _imageWriter = imageWriter;
        }

        [HttpGet]
        [Route("current")]
        [ProducesResponseType(typeof(SimpleUserViewModel), statusCode: 200)]
        public async Task<IActionResult> GetCurrentUser(CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(User.GetId());
            return Ok(_mapper.Map<SimpleUserViewModel>(user));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserViewModel>), 200)]
        public async Task<IActionResult> GetUser(CancellationToken token)
        {
            var users = await _userManager.Users.ToListAsync(token);

            return Ok(_mapper.Map<List<UserViewModel>>(users));
        }

        [HttpGet("currentfull")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        public async Task<IActionResult> GetCurrentUserFullInfo(CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(User.GetId());
            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpPost]
        [Route("uploadavatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file is null)
                return BadRequest("You have nothing to do here!");

            string userId = User.GetId();

            var result =  await _imageWriter.UploadImage(file, userId);

            if (result.Contains("Invalid image file") || result.Contains("An error has been occurred when server try to save the image"))
                return BadRequest(result);

            var user = await _userManager.FindByIdAsync(userId);
            user.IsHasAvatar = true;

            await _userManager.UpdateAsync(user);

            return Ok("Save image for user successed!");
        }

        [HttpGet]
        [Route("getavatar")]
        public async Task<IActionResult> GetAvatar()
        {
            var user = await _userManager.FindByIdAsync(User.GetId());

            if(user.IsHasAvatar)
            {
                Byte[] b = System.IO.File.ReadAllBytes(ImageProcessing.savePath + "/avatar_" + user.Id + ".image");   // You can use your own method over here.         
                return File(b, "image/jpeg");
            }

            return BadRequest("Nothing to show here!");
        }

        [HttpPost]
        [Route("updatebasicinfo")]
        public async Task<IActionResult> UpdateBasicInfo([FromBody] UserViewModel inputUserInfo)
        {
            var user = await _context.Users.SingleOrDefaultAsync(t => t.Id == inputUserInfo.Id);
            if(user != null)
            {
                user.FirstName = inputUserInfo.FirstName;
                user.MiddleName = inputUserInfo.MiddleName;
                user.LastName = inputUserInfo.LastName;
                user.Gender = inputUserInfo.Gender;
                user.EmployeeCode = inputUserInfo.EmployeeCode;
                user.PhoneNumber = inputUserInfo.PhoneNumber;
                user.VietnameseName = inputUserInfo.VietnameseName;
                user.EthnicRace = inputUserInfo.EthnicRace;
                user.IdCardNo = inputUserInfo.IdCardNo;
                user.Nationality = inputUserInfo.Nationality;
                user.MaritalStatus = inputUserInfo.MaritalStatus;
                user.BirthplaceCity = inputUserInfo.BirthplaceCity;
                user.IssuedDate = inputUserInfo.IssuedDate;
                user.IssuedPlace = inputUserInfo.IssuedPlace;

                await _context.SaveChangesAsync();
                return Ok(user);
            }
            else
            {
                return BadRequest("User not found!");
            }
        }

        [HttpGet]
        [Route("getuserbyaccountdomain")]
        [ProducesResponseType(typeof(UserViewModel), statusCode: 200)]
        public async Task<IActionResult> GetAllUserOfAccountDomain(string accDomainId)
        {
            List<User> users_raw = await _context.Users.ToListAsync();
            List<User> UserReturn = new List<User>();
            foreach(User user in users_raw)
            {
                foreach(var accdomain in user.UserAccountDomains)
                {
                    if(accdomain.AccountDomainId == accDomainId && accdomain.IsActive == true)
                    {
                        UserReturn.Add(user);
                        break;
                    }
                }
            }
            return Ok(_mapper.Map<List<UserViewModel>>(UserReturn));
        }

        [HttpPost]
        [Route("updateuseraccountdomain")]
        public async Task<IActionResult> UpdateUserAccountDomain([FromBody] UserAccountDomain newUserAccountDomain)
        {
            var result = await _context.UserAccountDomain.FirstOrDefaultAsync(x => x.UserId == newUserAccountDomain.UserId &&
                                                                                   x.AccountDomainId == newUserAccountDomain.AccountDomainId);

            if (result == null)
            {
                await _context.UserAccountDomain.AddAsync(newUserAccountDomain);
                await _context.SaveChangesAsync();
            }
            else
            {
                result.StartDate = newUserAccountDomain.StartDate;
                result.EndDate = newUserAccountDomain.EndDate;
                result.IsActive = newUserAccountDomain.IsActive;

                await _context.SaveChangesAsync();
            }
            
            return Ok(newUserAccountDomain);
        }

        [HttpGet]
        [Route("getcurrentuseraccountdomain")]
        public async Task<IActionResult> GetCurrentUserAccountDomain(CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(User.GetId());
            var accDomainId = "";
            foreach(var usrAccDomain in user.UserAccountDomains)
            {
                if(usrAccDomain.IsActive)
                {
                    accDomainId = usrAccDomain.AccountDomainId;
                    break;
                }
            }

            if (String.IsNullOrEmpty(accDomainId))
                return BadRequest("User hasn't assigned to any Account Domain");

            var accDomain = await _context.AccountDomains.FirstOrDefaultAsync(x => x.Id == accDomainId);
            return Ok(accDomain);
        }

        [HttpGet]
        [Route("getuser")]
        [ProducesResponseType(typeof(UserViewModel), statusCode: 200)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(_mapper.Map<UserViewModel>(user));
        }
    }
}
