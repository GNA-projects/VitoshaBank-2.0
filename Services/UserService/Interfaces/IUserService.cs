using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;

namespace VitoshaBank.Services.UserService.Interfaces
{
    public interface IUserService
    {
        public Task<ActionResult<MessageModel>> CreateUser(ClaimsPrincipal currentUser, Users user, IConfiguration _config);
        public Task<ActionResult<IEnumerable<Users>>> GetAllUsers(ClaimsPrincipal currentUser);
        public Task<ActionResult<Users>> GetUser(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> GetUsername(string username);
        public Task<ActionResult<MessageModel>> AdminCheck(string username);
        public Task<ActionResult<MessageModel>> LoginUser(Users userLogin, IConfiguration _config);
        public Task<ActionResult<MessageModel>> ChangePassword(string username, string newPassword);
        public Task<ActionResult> VerifyAccount(string activationCode);
        public Task<ActionResult<MessageModel>> DeleteUser(ClaimsPrincipal currentUser, string username);
    }
}
