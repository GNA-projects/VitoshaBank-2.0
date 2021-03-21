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
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.UserService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BankSystemContext dbContext;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IDepositService _depositService;
        //private readonly IBankAccountService _bankAccountService;
        //private readonly IDebitCardService _debitCardService;
        //private readonly ICreditService _creditService;
        //private readonly IWalletsService _walletService;
        //private readonly ISupportTicketService _ticketService;
        

        public AdminController(BankSystemContext context,  IConfiguration config, IUserService usersService, IDepositService depositService)
        {
            dbContext = context;
            _userService = usersService;
            _depositService = depositService;
            _config = config;
        }

        [HttpGet("authme")]
        [Authorize]
        public async Task<ActionResult> AuthMe()
        {
            var currentUser = HttpContext.User;

            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }
            if (role == "Admin")
            {
                return Ok();
            }
            else { return Unauthorized(); }
        }
        [HttpPost("create/user")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> CreateUser(UserRequestModel requestModel)
        {
            //need user(Firstname, Lastname, username, password, birthdate, email)
            var currentUser = HttpContext.User;
            return await _userService.CreateUser(currentUser, requestModel,  _config,dbContext);
        }
        [HttpGet("get/user/{username}")]
        [Authorize]
        public async Task<ActionResult<Users>> GetUser(string username)
        {
            var currentUser = HttpContext.User;
            return await _userService.GetUser(currentUser, username,dbContext);
        }
        [HttpDelete("delete/user")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteUser(UserRequestModel requestModel)
        {
            //need username
            var currentUser = HttpContext.User;
            return await _userService.DeleteUser(currentUser, requestModel,dbContext);
        }
        [HttpPost("create/deposit")]
        [Authorize]
        //need Deposit(amount, term of payment), username
        public async Task<ActionResult<MessageModel>> CreateDeposit(DepositRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _depositService.CreateDeposit(currentUser, requestModel, _config,dbContext);
        }
        [HttpDelete("delete/deposit")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteBankAccount(DepositRequestModel requestModel)
        {
            //need username and deposit
            var currentUser = HttpContext.User;
            return await _depositService.DeleteDeposit(currentUser,  requestModel,dbContext);
        }
    }
}
