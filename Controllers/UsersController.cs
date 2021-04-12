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
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.UserService;
using VitoshaBank.Services.UserService.Interfaces;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService _userService;
        private readonly IDepositsService _depositService;
        private readonly ICreditsService _creditService;
        private readonly IWalletsService _walletService;
        private readonly IChargeAccountsService _chargeAccount;
        public UsersController(IUsersService userService, IChargeAccountsService chargeAccountsService, IDepositsService depositsService, ICreditsService creditsService, IWalletsService walletsService)
        {
            _userService = userService;
            _chargeAccount = chargeAccountsService;
            _depositService = depositsService;
            _creditService = creditsService;
            _walletService = walletsService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            //return all users
            var currentUser = HttpContext.User;
            return await _userService.GetAllUsers(currentUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModel)
        {
            //need user(passowrd, email/username
            return await _userService.LoginUser(requestModel);
        }

        [HttpPut("changepass")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> ChangePassword(UserRequestModel requestModel)
        {
            //need password
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.ChangePassword(username, requestModel);
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
        [HttpGet("accounts")]
        [Authorize]
        public async Task<ActionResult<UserAccResponseModel>> GetAccounts()
        {
            //return all users
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _userService.GetAllUserBankAccounts(currentUser, username,_chargeAccount,_depositService,_creditService,_walletService);
        }
    }
}
