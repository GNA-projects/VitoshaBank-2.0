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
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.UserService.Interfaces;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        private readonly IUsersService _userService;
        private readonly IDepositsService _depositService;
        //private readonly IBankAccountService _bankAccountService;
        //private readonly IDebitCardService _debitCardService;
        private readonly ICreditsService _creditService;
        private readonly IWalletsService _walletService;
        //private readonly ISupportTicketService _ticketService;
        

        public AdminController(IUsersService usersService, IDepositsService depositService, ICreditService creditService, IWalletsService walletsService)
        {
            _userService = usersService;
            _creditService = creditService;
            _walletService = walletsService;
            _depositService = depositService;
        }

        [HttpGet("authme")]
        [Authorize]
        public ActionResult AuthMe()
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
            return await _userService.CreateUser(currentUser, requestModel);
        }
        [HttpGet("get/user/{username}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var currentUser = HttpContext.User;
            return await _userService.GetUser(currentUser, username);
        }
        [HttpDelete("delete/user")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteUser(UserRequestModel requestModel)
        {
            //need username
            var currentUser = HttpContext.User;
            return await _userService.DeleteUser(currentUser, requestModel);
        }
        [HttpPost("create/deposit")]
        [Authorize]
        //need Deposit(amount, term of payment), username
        public async Task<ActionResult<MessageModel>> CreateDeposit(DepositRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _depositService.CreateDeposit(currentUser, requestModel);
        }
        [HttpDelete("delete/deposit")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteBankAccount(DepositRequestModel requestModel)
        {
            //need username and deposit
            var currentUser = HttpContext.User;
            return await _depositService.DeleteDeposit(currentUser,  requestModel);
        }

        [HttpPost("create/wallet")]
        [Authorize]
        //need wallet(Amount), username
        public async Task<ActionResult<MessageModel>> CreateWallet(WalletRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _walletService.CreateWallet(currentUser, requestModel);
        }
        [HttpDelete("delete/wallet")]
        [Authorize]
        //need wallet(Iban), username
        public async Task<ActionResult<MessageModel>> DeleteWallet(WalletRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _walletService.DeleteWallet(currentUser, requestModel);
        }
        [HttpPost("create/credit")]
        [Authorize]
        //need Credit(amount), period, username
        public async Task<ActionResult<MessageModel>> CreateCredit(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _creditService.CreateCredit(currentUser, requestModel);
        }
        [HttpDelete("delete/credit")]
        [Authorize]
        //need username 
        public async Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _creditService.DeleteCredit(requestModel,currentUser);
        }
    }
}
