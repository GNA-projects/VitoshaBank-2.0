﻿using Microsoft.AspNetCore.Mvc;
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
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.IbanGenereatorService;
using VitoshaBank.Services.InterestService;
using VitoshaBank.Services.InterestService.Interfaces;
using VitoshaBank.Services.TransactionService.Interfaces;

namespace VitoshaBank.Services.CreditService
{
    public class CreditsService : ControllerBase, ICreditsService
    {
        private readonly BankSystemContext dbContext;
        private readonly IConfiguration config;
        private readonly ITransactionsService _transactionsService;
        private readonly ICreditPayOff _creditPay;
        MessageModel responseMessage = new MessageModel();
        public CreditsService(BankSystemContext context, IConfiguration _config, ITransactionsService transactionsService, ICreditPayOff creditPay)
        {
            dbContext = context;
            config = _config;
            _transactionsService = transactionsService;
            _creditPay = creditPay;
        }
        public async Task<ActionResult<MessageModel>> CreateCredit(ClaimsPrincipal currentUser, CreditRequestModel requestModel)
        {

            string role = "";
            string username = requestModel.Username;
            Credit credit = requestModel.Credit;
            int period = requestModel.Period;
            BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (userAuthenticate != null)
                {
                    if (dbContext.Credits.Where(x => x.UserId != userAuthenticate.Id).Count() < 3)
                    {
                        if (ValidateUser(userAuthenticate) && ValidateCredit(credit))
                        {

                            credit.Iban = IBANGenerator.GenerateIBANInVitoshaBank("Credit", dbContext);
                            credit.Interest = 6.9m;
                            credit.CreditAmount = CalculateInterest.CalculateCreditAmount(credit.Amount, period, credit.Interest);
                            credit.Instalment = CalculateInterest.CalculateInstalment(credit.CreditAmount, credit.Interest, period);
                            credit.CreditAmountLeft = credit.CreditAmount;
                            credit.PaymentDate = DateTime.Now.AddMonths(1);
                            credit.UserId = userAuthenticate.Id;
                            await dbContext.AddAsync(credit);
                            await dbContext.SaveChangesAsync();


                            SendEmail(userAuthenticate.Email);
                            responseMessage.Message = "Credit created succesfully!";
                            return StatusCode(200, responseMessage);
                        }
                        else if (ValidateUser(userAuthenticate) == false)
                        {
                            responseMessage.Message = "User not found!";
                            return StatusCode(404, responseMessage);
                        }
                        else if (ValidateCredit(credit) == false)
                        {
                            responseMessage.Message = "Don't put negative value!";
                            return StatusCode(400, responseMessage);
                        }
                    }

                }

                responseMessage.Message = "User already has 3 active credits!";
                return StatusCode(400, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<ICollection<CreditResponseModel>>> GetCreditInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                var userCredits = dbContext.Credits.Where(x => x.UserId == userAuthenticate.Id).ToList();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    List<CreditResponseModel> responseModels = new List<CreditResponseModel>();
                    foreach (var creditRef in userCredits)
                    {
                        CreditResponseModel creditResponseModel = new CreditResponseModel();
                        creditResponseModel.IBAN = creditRef.Iban;
                        creditResponseModel.Amount = Math.Round(creditRef.Amount, 2);
                        creditResponseModel.Instalment = creditRef.Instalment;
                        creditResponseModel.CreditAmount = creditRef.CreditAmountLeft;

                        responseModels.Add(creditResponseModel);
                    }

                    if (responseModels.Count > 0)
                    {
                        return StatusCode(200, responseModels.OrderBy(x=>x.IBAN));
                    }

                    responseMessage.Message = "You don't have a Credit!";
                    return StatusCode(400, responseMessage);
                }
            }
            return null;
        }
        public async Task<ActionResult<MessageModel>> GetPayOffInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                CreditResponseModel creditResponseModel = new CreditResponseModel();
                UserAccResponseModel userCredits = new UserAccResponseModel();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    ChargeAccount chargeAcc = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    bool hasDeleted = false;
                    foreach (var creditRef in dbContext.Credits.Where(x => x.UserId == userAuthenticate.Id))
                    {
                        var credit = creditRef;
                        if (credit == null)
                        {
                            responseMessage.Message = "You don't have a Credit";
                            return StatusCode(400, responseMessage);
                        }


                        await GetCreditPayOff(credit, username);
                        if (credit.CreditAmountLeft == 0 && credit.CreditAmount > 0)
                        {
                            CreditRequestModel requestModel = new CreditRequestModel();
                            requestModel.Credit = credit;
                            requestModel.Username = username;
                            responseMessage.Message = "You have payed your Credit!";
                            await DeleteCreditFromPayOff(credit, chargeAcc);
                            hasDeleted = true;
                        }

                    }
                    await dbContext.SaveChangesAsync();
                    if (hasDeleted)
                    {
                        responseMessage.Message = ($"Successfully payed montly pay off! Credit payed successfully! The left amount from the  credit is transfered to Bank Account with Iban: {chargeAcc.Iban}");
                        return StatusCode(200, responseMessage);
                    }
                    responseMessage.Message = "Successfully payed montly pay off!";
                    return StatusCode(200, responseMessage);
                }
            }

            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        private async Task<ActionResult<MessageModel>> DeleteCreditFromPayOff(Credit credit, ChargeAccount chargeAccount)
        {

            chargeAccount.Amount += credit.Amount;
            responseMessage.Message = ($"Credit payed successfully! The left amount from the  credit is transfered to Bank Account with Iban: {chargeAccount.Iban}");
            dbContext.Remove(credit);
            return StatusCode(200, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> SimulatePurchase(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username)
        {
            //amount credit product reciever
            var credit = requestModel.Credit;
            decimal amount = requestModel.Amount;
            var product = requestModel.Product;
            var reciever = requestModel.Reciever;
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            Credit creditExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    try
                    {
                        creditExists = await dbContext.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Credit not found";
                        return StatusCode(404, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (creditExists != null)
                {
                    try
                    {
                        if (ValidateCreditAmount(amount, creditExists) && ValidateCredit(creditExists))
                        {
                            creditExists.Amount = creditExists.Amount - amount;
                            Transaction transaction = new Transaction();
                            transaction.SenderAccountInfo = creditExists.Iban;
                            transaction.RecieverAccountInfo = reciever;
                            await _transactionsService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Purchasing {product}");
                            await dbContext.SaveChangesAsync();
                            responseMessage.Message = $"Succesfully purhcased {product}.";
                            return StatusCode(200, responseMessage);
                        }
                        else if (ValidateCreditAmount(amount, credit) == false)
                        {
                            responseMessage.Message = "Invalid payment amount!";
                            return StatusCode(400, responseMessage);
                        }
                        else if (ValidateCredit(creditExists) == false)
                        {
                            responseMessage.Message = "You don't have enough money in Charge account!";
                            return StatusCode(406, responseMessage);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Iban Invalid! Credit not found";
                        return StatusCode(404, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "Invalid Credit! Iban not found!";
                    return StatusCode(404, responseMessage);
                }
            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);

        }
        public async Task<ActionResult<MessageModel>> AddMoney(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username)
        {
            //credit amount
            var credit = requestModel.Credit;
            var amount = requestModel.Amount;
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            Credit creditsExists = null;
            ChargeAccount bankAccounts = requestModel.ChargeAccount;
            ChargeAccount bankAccExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    try
                    {
                        creditsExists = await dbContext.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);

                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Credit not found";
                        return StatusCode(404, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (creditsExists != null)
                {
                    try
                    {
                        bankAccExists = dbContext.ChargeAccounts.FirstOrDefault(x => x.Iban == bankAccounts.Iban);
                        return await ValidateDepositAmountAndCredit(userAuthenticate, creditsExists, currentUser, amount, bankAccExists);

                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Invalid Charge Account! Iban not found!";
                        return StatusCode(404, responseMessage);
                    }

                }
                else
                {
                    responseMessage.Message = "Credit not found";
                    return StatusCode(404, responseMessage);
                }
            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> Withdraw(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, string reciever)
        {
            //credit amount
            var credit = requestModel.Credit;
            var amount = requestModel.Amount;
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            Credit creditExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    try
                    {
                        creditExists = await dbContext.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Credit not found";
                        return StatusCode(404, responseMessage);
                    }

                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (creditExists != null)
                {
                    try
                    {
                        if (ValidateDepositAmountBankAccount(amount) && ValidateCredit(creditExists) && ValidateMinAmount(creditExists, amount))
                        {
                            creditExists.Amount = creditExists.Amount - amount;
                            Transaction transactions = new Transaction();
                            transactions.SenderAccountInfo = credit.Iban;
                            transactions.RecieverAccountInfo = reciever;
                            await _transactionsService.CreateTransaction(userAuthenticate, currentUser, amount, transactions, $"Withdrawing {amount} leva");
                            await dbContext.SaveChangesAsync();
                            responseMessage.Message = $"Succesfully withdrawed {amount} leva.";
                            return StatusCode(200, responseMessage);
                        }
                        else if (ValidateDepositAmountBankAccount(amount) == false)
                        {
                            responseMessage.Message = "Invalid payment amount!";
                            return StatusCode(400, responseMessage);
                        }
                        else if (ValidateCredit(creditExists) == false)
                        {
                            responseMessage.Message = "You don't have enough money in Credit Account!";
                            return StatusCode(406, responseMessage);
                        }
                        else if (ValidateMinAmount(creditExists, amount) == false)
                        {
                            responseMessage.Message = "The amount is bigger than Credit Account's amount!";
                            return StatusCode(406, responseMessage);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Iban Invalid! Credit not found";
                        return StatusCode(404, responseMessage);

                    }

                }
                else
                {
                    responseMessage.Message = "Iban Invalid! Credit not found";
                    return StatusCode(404, responseMessage);
                }
            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel, ClaimsPrincipal currentUser)
        {
            string role = "";
            var username = requestModel.Username;
            var credit = requestModel.Credit;
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                Credit creditsExists = null;

                if (user != null)
                {
                    try
                    {
                        creditsExists = await dbContext.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
                    }
                    catch (NullReferenceException)
                    {
                        responseMessage.Message = "Credit not found";
                        return StatusCode(404, responseMessage);
                    }
                }

                if (user == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else if (creditsExists == null)
                {
                    responseMessage.Message = "User deosn't have a credit!";
                    return StatusCode(400, responseMessage);
                }

                dbContext.Credits.Remove(creditsExists);
                await dbContext.SaveChangesAsync();

                responseMessage.Message = "Credit deleted successfully!";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        private bool ValidateMinAmount(Credit credit, decimal amount)
        {
            if (amount <= credit.Amount)
            {
                return true;
            }
            return false;
        }
        private bool ValidateDepositAmountBankAccount(decimal amount)
        {
            if (amount >= 10)
            {
                return true;
            }
            return false;
        }
        private bool ValidateCreditAmount(decimal amount, Credit credit)
        {
            if (credit.Amount < amount)
            {
                return false;
            }
            return true;
        }
        private bool ValidateCredit(Credit credits)
        {
            if (credits.Amount < 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUser(User user)
        {
            if (user != null)
            {
                return true;
            }
            return false;
        }

        private async Task<ActionResult> ValidateDepositAmountAndCredit(User userAuthenticate, Credit creditExists, ClaimsPrincipal currentUser, decimal amount, ChargeAccount bankAccount)
        {
            if (amount < 0)
            {
                responseMessage.Message = "Invalid payment amount!";
                return StatusCode(400, responseMessage);
            }
            else if (amount == 0)
            {
                responseMessage.Message = "Put amount more than 0.00lv";
                return StatusCode(400, responseMessage);
            }
            else
            {
                if (bankAccount != null && bankAccount.Amount > amount)
                {
                    creditExists.Amount = creditExists.Amount + amount;
                    bankAccount.Amount = bankAccount.Amount - amount;
                    Transaction transaction = new Transaction();
                    transaction.SenderAccountInfo = bankAccount.Iban;
                    transaction.RecieverAccountInfo = creditExists.Iban;
                    await _transactionsService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Depositing money in Credit Account");
                    await dbContext.SaveChangesAsync();
                }
                else if (bankAccount.Amount < amount)
                {
                    responseMessage.Message = "You don't have enough money in Charge account!";
                    return StatusCode(406, responseMessage);
                }
                else if (bankAccount == null)
                {
                    responseMessage.Message = "You don't have a Charge account";
                    return StatusCode(400, responseMessage);
                }
            }
            responseMessage.Message = $"Succesfully deposited {amount} leva.";
            return StatusCode(200, responseMessage);
        }
        private async Task<ActionResult<MessageModel>> GetCreditPayOff(Credit credit, string username)
        {
            while (DateTime.Now >= credit.PaymentDate)
            {
                if (credit.Instalment <= credit.Amount)
                {

                    credit.Amount = credit.Amount - credit.Instalment;
                    credit.CreditAmountLeft = credit.CreditAmountLeft - credit.Instalment;
                    credit.PaymentDate = credit.PaymentDate.AddMonths(1);

                    responseMessage.Message = "Credit instalment payed off successfully from Credit Account!";
                    return StatusCode(200, responseMessage);
                }
                else
                {
                    int count = 1;
                    var chargeAccountsCollection = dbContext.ChargeAccounts.Where(x => x.UserId == dbContext.Users.FirstOrDefault(z => z.Username == username).Id);
                    foreach (var chargeAccountReff in chargeAccountsCollection)
                    {
                        ChargeAccount chargeAccount = chargeAccountReff;
                        if (credit.Instalment <= chargeAccount.Amount)
                        {
                            chargeAccount.Amount = chargeAccount.Amount - credit.Instalment;
                            credit.CreditAmountLeft = credit.CreditAmountLeft - credit.Instalment;
                            credit.PaymentDate = credit.PaymentDate.AddMonths(1);
                            await dbContext.SaveChangesAsync();
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
        private void SendEmail(string email)
        {
            var fromMail = new MailAddress(config["Email:Email"], $"Credit account created");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = config["Pass:Pass"];
            string subject = "Your credit account is successfully created";
            string body = "<br/><br/>We are excited to tell you that your credit account is created succesfully. You can use it instantly.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, frontEmailPassowrd)

            };
            using (var message = new MailMessage(fromMail, toMail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

    }
}
