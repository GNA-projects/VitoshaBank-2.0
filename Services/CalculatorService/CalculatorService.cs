using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.BcryptHasherService;
using VitoshaBank.Services.CalculatorService.Interfaces;
using VitoshaBank.Services.IbanGenereatorService;
using VitoshaBank.Services.InterestService;

namespace VitoshaBank.Services.CalculatorService
{
    public class CalculatorService : ControllerBase, ICalculatorService
    {
        MessageModel responseMessage = new MessageModel();
        public async Task<ActionResult<MessageModel>> Calculate(string curr1, string curr2)
        {
            responseMessage.Message = curr1 + curr2;
            return responseMessage;
        }   

    }
}
