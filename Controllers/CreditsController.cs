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
using VitoshaBank.Services.CreditService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/credits")]
    [ApiController]
    public class CreditController : ControllerBase
    {

        private readonly ICreditsService _creditService;

        public CreditController(ICreditsService creditService)
        {
            _creditService = creditService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<CreditResponseModel>>> GetCreditInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _creditService.GetCreditInfo(currentUser, username);
        }
        [HttpGet("check")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> GetPayOffInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _creditService.GetPayOffInfo(currentUser, username);
        }

        [HttpPut("purchase")]
        [Authorize]

        public async Task<ActionResult<MessageModel>> Purchase(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _creditService.SimulatePurchase(requestModel, currentUser, username);
        }

        [HttpPut("deposit")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Deposit(CreditRequestModel requestModel)
        {
            
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _creditService.AddMoney(requestModel, currentUser, username);
        }
        [HttpPut("withdraw")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> Withdraw(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _creditService.Withdraw(requestModel, currentUser, username, "User in ATM");
        }

    }
}
