using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Services.UserService;
using VitoshaBank.Services.UserService.Interfaces;

namespace VitoshaBank.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsersService _userService;
        private readonly BankSystemContext dbContext;

        public UsersController(BankSystemContext context,IConfiguration config, IUsersService userService)
        {
            dbContext = context;
            _config = config;
            _userService = userService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            //return all users
            var currentUser = HttpContext.User;
            return await _userService.GetAllUsers(currentUser,dbContext);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModel)
        {
            //need user(passowrd, email/username
            return await _userService.LoginUser(requestModel, _config,dbContext);
        }

        [HttpPut("changepass")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> ChangePassword(UserRequestModel requestModel)
        {
            //need password
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.ChangePassword(username, requestModel, dbContext);
        }

        [HttpGet("activateaccount/{id}")]
        public async Task<ActionResult<MessageModel>> VeryfiyUserAccount(string id)
        {
            return await _userService.VerifyAccount(id, dbContext);
        }
        [HttpGet("username")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> GetUsername()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.GetUsername(username,dbContext);
        }
        [HttpGet("authenticate")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> AutihentivcateUser()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.AdminCheck(username,dbContext);
        }

    }
}
