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

namespace VitoshaBank.Services.CreditService.Interfaces
{
    public interface ICreditsService
    {

        Task<ActionResult<ICollection<CreditResponseModel>>> GetCreditInfo(ClaimsPrincipal currentUser, string username);

        Task<ActionResult<MessageModel>> GetPayOffInfo(ClaimsPrincipal currentUser, string username);
        Task<ActionResult<MessageModel>> SimulatePurchase(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        Task<ActionResult<MessageModel>> AddMoney(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        Task<ActionResult<MessageModel>> Withdraw(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, string v);
        Task<ActionResult<MessageModel>> CreateCredit(ClaimsPrincipal currentUser, CreditRequestModel requestModel);

        Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel,ClaimsPrincipal currentUser);
    }
}
