using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.MessageModels;

namespace VitoshaBank.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsersService _userService;
        
        public UsersController(IConfiguration config, IUsersService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            //return all users
            var currentUser = HttpContext.User;
            return await _userService.GetAllUsers(currentUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModel)
        {
            return await _userService.LoginUser(requestModel.User, _config);
        }

        [HttpPut("changepass")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> ChangePassword(UserRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.ChangePassword(username, requestModel.User.Password);
        }

        [HttpGet("activateaccount/{id}")]
        public async Task<ActionResult<MessageModel>> VeryfiyUserAccount(string id)
        {
            return await _userService.VerifyAccount(id);
        }
        [HttpGet("username")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> GetUsername()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.GetUsername(username);
        }
        [HttpGet("authenticate")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> AutihentivcateUser()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.AdminCheck(username);
        }

    }
}
