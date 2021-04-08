using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.DebitCardService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/debits")]
    [ApiController]
    public class DebitCardsController : ControllerBase
    {

        private readonly IDebitCardsService _debitCardService;
        private readonly IChargeAccountsService _chargeAccountService;
        //private readonly ITransactionService _transactionService;

        public DebitCardsController(IDebitCardsService debitCardService, IChargeAccountsService bankaccService /*ITransactionService transactionService*/)
        {
            _debitCardService = debitCardService;
            _chargeAccountService = bankaccService;
           // _transactionService = transactionService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<DebitCardResponseModel>>> GetDebitCardInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _debitCardService.GetDebitCardInfo(currentUser, username);
        }

        [HttpPut("deposit")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Deposit(DebitCardRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _debitCardService.AddMoney(requestModel.Card.CardNumber, requestModel.Card.Cvv, requestModel.Card.CardExpirationDate, currentUser, username, requestModel.Amount, _chargeAccountService);
        }

        [HttpPut("purchase")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Purchase(DebitCardRequestModel requestModel)
        {

            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _debitCardService.SimulatePurchase(requestModel.Card.CardNumber, requestModel.Card.Cvv, requestModel.Card.CardExpirationDate, requestModel.Product, currentUser, username, requestModel.Amount, requestModel.Reciever, _chargeAccountService);
        }

        [HttpPut("withdraw")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Withdraw(DebitCardRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _debitCardService.Withdraw(requestModel.Card.CardNumber, requestModel.Card.Cvv, requestModel.Card.CardExpirationDate, currentUser, username, requestModel.Amount, requestModel.Reciever, _chargeAccountService);
        }
    }
}
