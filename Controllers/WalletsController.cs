using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletsController : ControllerBase
    {
        private readonly BankSystemContext dbContext;
        private readonly IWalletsService _walletService;
        public WalletsController(BankSystemContext context, IWalletsService walletService)
        {
            dbContext = context;
            _walletService = walletService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<WalletResponseModel>>> GetWalletInfo()
        {
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _walletService.GetWalletsInfo(currentUser, username, dbContext);
        }

        [HttpPut("deposit")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> DepositInWallet(WalletRequestModel requestModel)
        {
            //Wallet(IBAN), BankAcc(IBAN), Amount
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _walletService.AddMoney(requestModel, currentUser, username, dbContext);
        }

        [HttpPut("purchase")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> PurchaseWithWallet(WalletRequestModel requestModel)
        {
            //Product, Reciever, Amount, Wallet(Iban, CardNumber, CardExpirationDate, CVV)
            var currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Username").Value;
            return await _walletService.SimulatePurchase(requestModel, currentUser, username, dbContext);
        }
    }
}
