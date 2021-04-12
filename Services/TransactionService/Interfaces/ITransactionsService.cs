using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.ResponseModels;

namespace VitoshaBank.Services.TransactionService.Interfaces
{
    public interface ITransactionsService
    {
        public Task<ActionResult> CreateTransaction(User user, ClaimsPrincipal currentUser, decimal amount, Transaction transaction, string reason);

        public Task<ActionResult<GetTransactionsResponseModel>> GetTransactionInfo(ClaimsPrincipal currentUser, string username);
    }
}
