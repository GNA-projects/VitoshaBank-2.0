using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;

namespace VitoshaBank.Services.InterestService.Interfaces
{
     public interface ICreditPayOff
    {
        Task<ActionResult<MessageModel>> GetCreditPayOff(Credit credit, string username);
    }
}
