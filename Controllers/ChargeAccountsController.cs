using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.DebitCardService.Interfaces;

namespace VitoshaBank.Controllers
{
    [Route("api/charges")]
    [ApiController]
    public class ChargeAccountsController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IChargeAccountsService _chargeAccountsService;
        private readonly IDebitCardsService _debitCardsService;


       
        
    }
}
