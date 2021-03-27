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
    public interface ICreditService
    {
        Task<ActionResult<ICollection<CreditResponseModel>>> GetCreditInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext);
        Task<ActionResult<CreditResponseModel>> GetPayOffInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext);
        Task<ActionResult<MessageModel>> SimulatePurchase(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext dbContext);
        Task<ActionResult<MessageModel>> AddMoney(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext dbContext);
        Task<ActionResult<MessageModel>> Withdraw(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, string v, BankSystemContext dbContext);
        Task<ActionResult<MessageModel>> CreateCredit(ClaimsPrincipal currentUser, CreditRequestModel requestModel, IConfiguration _config, BankSystemContext dbContext);
        Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel,ClaimsPrincipal currentUser,  BankSystemContext dbContext);
    }
}
