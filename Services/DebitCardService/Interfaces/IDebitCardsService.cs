using Microsoft.AspNetCore.Mvc;
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

namespace VitoshaBank.Services.DebitCardService.Interfaces
{
    public interface IDebitCardsService
    {
        public  Task<ActionResult<MessageModel>> CreateDebitCard(ClaimsPrincipal currentUser, string username, ChargeAccount chargeAccount, Card card);
        public Task<ActionResult<ICollection<DebitCardResponseModel>>> GetDebitCardInfo(ClaimsPrincipal currentUser, string username);
        public  Task<ActionResult<MessageModel>> AddMoney(string cardNumber, string CVV, DateTime expireDate, ClaimsPrincipal currentUser, string username, decimal amount/*, ITransactionService _transactionService*/, IChargeAccountsService _chargeAccService);
        public  Task<ActionResult<MessageModel>> SimulatePurchase(string cardNumber, string CVV, DateTime expireDate, string product, ClaimsPrincipal currentUser, string username, decimal amount, string reciever/*, ITransactionService _transactionService*/, IChargeAccountsService _chargeAccService);
        public  Task<ActionResult<MessageModel>> Withdraw(string cardNumber, string CVV, DateTime expireDate, ClaimsPrincipal currentUser, string username, decimal amount, string reciever, /*ITransactionService _transactionService*/IChargeAccountsService _chargeAccService);
        public  Task<ActionResult<MessageModel>> DeleteDebitCard(ClaimsPrincipal currentUser, DebitCardRequestModel requestModel, string username);

    }
}
