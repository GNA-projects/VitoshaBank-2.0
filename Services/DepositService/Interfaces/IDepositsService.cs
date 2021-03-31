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

namespace VitoshaBank.Services.DepositService.Interfaces
{
    public interface IDepositsService
    {
        public Task<ActionResult<ICollection<DepositResponseModel>>> GetDepositsInfo(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<DepositResponseModel>> CheckDividentsPayments(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<DepositResponseModel>> GetDividentsInfo(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> CreateDeposit(ClaimsPrincipal currentUser, DepositRequestModel requestModel);
        public Task<ActionResult<MessageModel>> AddMoney(DepositRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> WithdrawMoney(DepositRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        public  Task<ActionResult<MessageModel>> DeleteDeposit(ClaimsPrincipal currentUser, DepositRequestModel requestModel);
    }
}
