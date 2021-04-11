using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Services.InterestService.Interfaces;

namespace VitoshaBank.Services.InterestService
{
    public class CreditPayOff : ControllerBase, ICreditPayOff
    {
        private readonly BankSystemContext _context;
        MessageModel responseMessage = new MessageModel();
        public CreditPayOff(BankSystemContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<MessageModel>> GetCreditPayOff(Credit credit, string username)
        {
            while (DateTime.Now >= credit.PaymentDate)
            {
                if (credit.Instalment <= credit.Amount)
                {
                    credit.Amount = credit.Amount - credit.Instalment;
                    credit.CreditAmountLeft = credit.CreditAmountLeft - credit.Instalment;
                    credit.PaymentDate = credit.PaymentDate.AddMonths(1);
                    _context.SaveChanges();
                    responseMessage.Message = "Credit instalment payed off successfully from Credit Account!";
                    return StatusCode(200, responseMessage);
                }
                else
                {
                    int count = 1;
                    var chargeAccountsCollection = _context.ChargeAccounts.Where(x => x.UserId == _context.Users.FirstOrDefault(z=>z.Username == username).Id);
                    foreach (var chargeAccountReff in chargeAccountsCollection)
                    {
                        ChargeAccount chargeAccount = chargeAccountReff;
                        if (credit.Instalment <= chargeAccount.Amount)
                        {
                            chargeAccount.Amount = chargeAccount.Amount - credit.Instalment;
                            credit.CreditAmountLeft = credit.CreditAmountLeft - credit.Instalment;
                            credit.PaymentDate = credit.PaymentDate.AddMonths(1);
                            await _context.SaveChangesAsync();
                            responseMessage.Message = "Credit instalment payed off successfully from Charge Account!";
                            return StatusCode(200, responseMessage);
                        }
                        else
                        {
                            if (count > chargeAccountsCollection.Count())
                            {
                                responseMessage.Message = "You don't have enough money to pay off Your instalment! Come to our office as soon as possible to discuss what happens from now on!";
                                return StatusCode(406, responseMessage);
                            }
                            count++;
                        }
                    }
                }
            }
            return null;
        }
    }
}
