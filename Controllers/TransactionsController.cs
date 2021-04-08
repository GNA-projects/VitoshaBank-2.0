using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.TransactionService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionService;
        public TransactionsController(ITransactionsService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetTransactionsResponseModel>> GetTransactionsInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _transactionService.GetTransactionInfo(currentUser, username);
        }
    }
}
