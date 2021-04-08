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
using VitoshaBank.Services.DebitCardService.Interfaces;

namespace VitoshaBank.Services.ChargeAccountService.Interfaces
{
    public interface IChargeAccountsService
    {
        public Task<ActionResult<MessageModel>> CreateChargeAccount(ClaimsPrincipal currentUser, ChargeAccountRequestModel requestModel, IDebitCardsService _debitCardService );
        public Task<ActionResult<ICollection<ChargeAccountResponseModel>>> GetBankAccountInfo(ClaimsPrincipal currentUser, string username);
        //public Task<ActionResult<MessageModel>> DepositMoney(ChargeAccount bankAccount, ClaimsPrincipal currentUser, string username, decimal amount, BankSystemContext _context, /*ITransactionService _transactionService*/ MessageModel messageModel);
        public  Task<ActionResult<MessageModel>> DepositMoney(ChargeAccount bankAccount, Deposit deposit, ClaimsPrincipal currentUser, string username, decimal amount /*ITransactionService _transactionService*/);
        public Task<ActionResult<MessageModel>> SimulatePurchase(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username /*ITransactionService _transation*/);
        public Task<ActionResult<MessageModel>> AddMoney(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> Withdraw(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> DeleteBankAccount(ClaimsPrincipal currentUser, ChargeAccountRequestModel requestModel);
    }
}
