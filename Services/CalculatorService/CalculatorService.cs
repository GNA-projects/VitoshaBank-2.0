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
            Dictionary<string, decimal> Currencies = new Dictionary<string, decimal>();

            Currencies.Add("BGN", 1);
            Currencies.Add("USD", 0.6024m);
            Currencies.Add("EUR", 0.5114m);
            Currencies.Add("JPY", 66.5623m); 
            Currencies.Add("CHF", 0.5671m);
            Currencies.Add("RUB", 45.8288m);
            Currencies.Add("AUD", 0.7893m);
            Currencies.Add("CNY", 3.9519m);
            Currencies.Add("INR", 44.1257m);
            Currencies.Add("TRY", 4.8854m);
            Currencies.Add("GBP", 0.4349m);
            Currencies.Add("RON", 2.5094m);
            Currencies.Add("RSD", 60.0205m);
            Currencies.Add("MKD", 31.4318m);

            responseMessage.Message = "";
            return StatusCode(200, responseMessage);
        }   

    }
}
