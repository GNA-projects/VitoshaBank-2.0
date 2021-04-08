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

namespace VitoshaBank.Services.WalletService.Interfaces
{
    public interface IWalletsService
    {
        public Task<ActionResult<ICollection<WalletResponseModel>>> GetWalletsInfo(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> CreateWallet(ClaimsPrincipal currentUser, WalletRequestModel requestModel);
        public Task<ActionResult<MessageModel>> AddMoney(WalletRequestModel requestModel, ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<MessageModel>> SimulatePurchase(WalletRequestModel requestModel, ClaimsPrincipal currentUser, string username /*ITransactionService _transation*/);
        public Task<ActionResult<MessageModel>> DeleteWallet(ClaimsPrincipal currentUser, WalletRequestModel requestModel);

    }
}
