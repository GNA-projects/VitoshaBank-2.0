﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;

namespace VitoshaBank.Services.Dividents
{
    public class DividentService : ControllerBase
    {
        MessageModel messageModel = new MessageModel();
        BankSystemContext dbContext = new BankSystemContext();
        public async Task<ActionResult<MessageModel>> GetDividentPayment(Data.DbModels.Deposits deposit)
        {
            if (DateTime.Now >= deposit.PaymentDate)
            {
                var dividentAmount = CalculateDividentService.GetDividentAmount(deposit.Amount, deposit.Divident, deposit.TermOfPayment);
                deposit.Amount = deposit.Amount + dividentAmount;
                deposit.PaymentDate.AddMonths(deposit.TermOfPayment);
                await dbContext.SaveChangesAsync();
                messageModel.Message = "Deposit divident applied successfully!";
                return StatusCode(200, messageModel);
            }
            return null;
        }
    }
}
