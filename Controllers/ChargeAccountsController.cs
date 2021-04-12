using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.DebitCardService;
using VitoshaBank.Services.DebitCardService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/charges")]
    [ApiController]
    public class ChargeAccountsController : ControllerBase
    {
        
        private readonly IChargeAccountsService _chargeAccService;
        private readonly IDebitCardsService _debitCardService;
        
       // private readonly ITransactionService _transactionService;

        public ChargeAccountsController(IChargeAccountsService bankAccountService, IDebitCardsService debitCardService)
        {
            _chargeAccService= bankAccountService;
            _debitCardService = debitCardService;

        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<ChargeAccountResponseModel>>> GetBankAccountInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _chargeAccService.GetBankAccountInfo(currentUser, username);
        }


        [HttpPut("fromdeposit")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> DepositInBankAcc(ChargeAccountRequestModel requestModel)
        {
            //amount = 0.50M;
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _chargeAccService.DepositMoney(requestModel.ChargeAccount,requestModel.Deposit, currentUser, username, requestModel.Amount);
        }

        [HttpPut("purchase")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Purchase(ChargeAccountRequestModel requestModel)
        {

            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _chargeAccService.SimulatePurchase(requestModel, currentUser, username);
        }

        [HttpPut("withdraw")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Withdraw(ChargeAccountRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _chargeAccService.Withdraw(requestModel, currentUser, username);
        }





    }
}
