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
        [ProducesResponseType(typeof(SimpleUserViewModel), statusCode:200)]
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
    }
}
