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
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Services.UserService.Interfaces
{
    public interface IUsersService
    {

        public Task<ActionResult<IEnumerable<User>>> GetAllUsers(ClaimsPrincipal currentUser);
        public Task<ActionResult<User>> GetUser(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> GetUsername(string username);
        public Task<ActionResult<MessageModel>> AdminCheck(string username);
        public Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModelt);
        public Task<ActionResult<MessageModel>> ChangePassword(string username, UserRequestModel requestModel);
        public Task<ActionResult> VerifyAccount(string activationCode);
        public Task<ActionResult<UserAccResponseModel>> GetAllUserBankAccounts(ClaimsPrincipal currentUser, string username, IChargeAccountsService chargeAccount, IDepositsService depositService, ICreditsService creditsService, IWalletsService walletsService);
        public Task<ActionResult<MessageModel>> DeleteUser(ClaimsPrincipal currentUser, UserRequestModel requestModel);
        public Task<ActionResult<MessageModel>> CreateUser(ClaimsPrincipal currentUser, UserRequestModel requestModel);


    }
}
