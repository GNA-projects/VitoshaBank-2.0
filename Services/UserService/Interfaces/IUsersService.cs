using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;

namespace VitoshaBank.Services.UserService.Interfaces
{
    public interface IUsersService
    {
        
        public Task<ActionResult<IEnumerable<Users>>> GetAllUsers(ClaimsPrincipal currentUser, BankSystemContext dbContext);
        public Task<ActionResult<Users>> GetUser(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext);
        public Task<ActionResult<MessageModel>> GetUsername(string username, BankSystemContext dbContext);
        public Task<ActionResult<MessageModel>> AdminCheck(string username, BankSystemContext dbContext);
        public Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModel, IConfiguration config, BankSystemContext dbContext);
        public Task<ActionResult<MessageModel>> ChangePassword(string username, UserRequestModel requestModel, BankSystemContext dbContext);
        public  Task<ActionResult> VerifyAccount(string activationCode, BankSystemContext dbContext);
        
        public Task<ActionResult<MessageModel>> DeleteUser(ClaimsPrincipal currentUser, UserRequestModel requestModel, BankSystemContext dbContext);
        public Task<ActionResult<MessageModel>> CreateUser(ClaimsPrincipal currentUser, UserRequestModel requestModel, IConfiguration config, BankSystemContext dbContext);
    }
}
