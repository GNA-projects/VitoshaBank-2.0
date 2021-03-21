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
using VitoshaBank.Services.DepositService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/deposits")]
    [ApiController]
    public class DepositsController : ControllerBase
    {
        private readonly BankSystemContext dbContext;
        private readonly IDepositsService _depositService;
        public DepositsController(BankSystemContext context, IDepositsService depositService)
        {
            dbContext = context;  
            _depositService = depositService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<DepositResponseModel>>> GetDepositInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _depositService.GetDepositsInfo(currentUser, username,dbContext);
        }

        [HttpPut("deposit")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> DepositMoney(DepositRequestModel requestModel)
        {
            //need from deposit(IBAN), BankAcc(IBAN),Username,Amount
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _depositService.AddMoney(requestModel, currentUser, username,dbContext);
        }
        [HttpPut("withdraw")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> WithDrawMoney(DepositRequestModel requestModel)
        {
            //need from deposit(IBAN), BankAcc(IBAN),Username,Amount
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _depositService.WithdrawMoney(requestModel, currentUser, username,dbContext);
        }
        [HttpGet("info")]
        [Authorize]
        public async Task<ActionResult<DepositResponseModel>> GetDividentInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _depositService.GetDividentsInfo(currentUser, username,dbContext);
        }
        [HttpGet("check")]
        [Authorize]
        public async Task<ActionResult<DepositResponseModel>> CheckDividentPayment()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _depositService.CheckDividentsPayments(currentUser, username,dbContext);
        }
    }
}
