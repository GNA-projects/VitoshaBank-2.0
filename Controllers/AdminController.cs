using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.DebitCardService.Interfaces;
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.SupportTicketService.Interfaces;
using VitoshaBank.Services.UserService.Interfaces;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        private readonly IUsersService _userService;
        private readonly IDepositsService _depositService;
        private readonly IChargeAccountsService _chargeAccountService;
        private readonly IDebitCardsService _debitCardService;
        private readonly ICreditsService _creditService;
        private readonly IWalletsService _walletService;
        private readonly ISupportTicketsService _supportTicketService;
        //private readonly ISupportTicketService _ticketService;
        

        public AdminController(IUsersService usersService, IDepositsService depositService, ICreditsService creditService, IWalletsService walletsService, IChargeAccountsService chargeAccountService, IDebitCardsService debitCardService, ISupportTicketsService supportTicketService)
        {
            _userService = usersService;
            _creditService = creditService;
            _walletService = walletsService;
            _depositService = depositService;
            _chargeAccountService = chargeAccountService;
            _debitCardService = debitCardService;
            _supportTicketService = supportTicketService;

        }

        [HttpGet("authme")]
        [Authorize]
        public ActionResult AuthMe()
        {
            var currentUser = HttpContext.User;

            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }
            if (role == "Admin")
            {
                return Ok();
            }
            else { return Unauthorized(); }
        }
        [HttpPost("create/user")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> CreateUser(UserRequestModel requestModel)
        {
            //need user(Firstname, Lastname, username, password, birthdate, email)
            var currentUser = HttpContext.User;
            return await _userService.CreateUser(currentUser, requestModel);
        }
        [HttpGet("get/user/{username}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var currentUser = HttpContext.User;
            return await _userService.GetUser(currentUser, username);
        }
        [HttpDelete("delete/user")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteUser(UserRequestModel requestModel)
        {
            //need username
            var currentUser = HttpContext.User;
            return await _userService.DeleteUser(currentUser, requestModel);
        }
        [HttpPost("create/deposit")]
        [Authorize]
        //need Deposit(amount, term of payment), username
        public async Task<ActionResult<MessageModel>> CreateDeposit(DepositRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _depositService.CreateDeposit(currentUser, requestModel);
        }
        [HttpDelete("delete/deposit")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteBankAccount(DepositRequestModel requestModel)
        {
            //need username and deposit
            var currentUser = HttpContext.User;
            return await _depositService.DeleteDeposit(currentUser,  requestModel);
        }

        [HttpPost("create/wallet")]
        [Authorize]
        //need wallet(Amount), username
        public async Task<ActionResult<MessageModel>> CreateWallet(WalletRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _walletService.CreateWallet(currentUser, requestModel);
        }
        [HttpDelete("delete/wallet")]
        [Authorize]
        //need wallet(Iban), username
        public async Task<ActionResult<MessageModel>> DeleteWallet(WalletRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _walletService.DeleteWallet(currentUser, requestModel);
        }
        [HttpPost("create/credit")]
        [Authorize]
        //need Credit(amount), period, username
        public async Task<ActionResult<MessageModel>> CreateCredit(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _creditService.CreateCredit(currentUser, requestModel);
        }

        [HttpDelete("delete/credit")]
        [Authorize]
        //need username 
        public async Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _creditService.DeleteCredit(requestModel,currentUser);
        }

        [HttpPost("create/debitcard")]
        [Authorize]
        //need bankaccount(IBAN), username, card()
        public async Task<ActionResult<MessageModel>> CreateDebitcard(DebitCardRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _debitCardService.CreateDebitCard(currentUser, requestModel.Username, requestModel.ChargeAccount, requestModel.Card);
        }

        [HttpDelete("delete/debitcard")]
        [Authorize]
        //need username
        public async Task<ActionResult<MessageModel>> DeleteDebitCard(DebitCardRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _debitCardService.DeleteDebitCard(currentUser, requestModel, requestModel.Username);
        }

        [HttpGet("get/support")]
        [Authorize]
        public async Task<ActionResult<ICollection<SupportTicketResponseModel>>> GetAllTickets()
        {
            var currentUser = HttpContext.User;
            return await _supportTicketService.GetAllTicketsInfo(currentUser);
        }

        [HttpPut("respond/support")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> RespondToTicket(SupportTicketRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _supportTicketService.GiveResponse(currentUser, requestModel.Ticket.Id);
        }

        [HttpPost("create/charge")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> CreateBankAccount(ChargeAccountRequestModel requestModel)
        {
            //need username, BankAccount(amount)
            var currentUser = HttpContext.User;
            return await _chargeAccountService.CreateChargeAccount(currentUser, requestModel, _debitCardService);
        }

        [HttpPut("addmoney/charge")]
        [Authorize]
        //need BankAccount(IBAN), username, amount
        public async Task<ActionResult<MessageModel>> AddMoneyInBankAccount(ChargeAccountRequestModel requestModel)
        {
            var currentUser = HttpContext.User;
            return await _chargeAccountService.AddMoney(requestModel, currentUser, requestModel.Username);
        }
        [HttpDelete("delete/charge")]
        [Authorize]
        public async Task<ActionResult<MessageModel>> DeleteBankAccount(ChargeAccountRequestModel requestModel)
        {
            //need username
            var currentUser = HttpContext.User;
            return await _chargeAccountService.DeleteBankAccount(currentUser, requestModel);
        }


    }
}
