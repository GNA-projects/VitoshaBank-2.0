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

namespace VitoshaBank.Services.CreditService
{
    public class CreditsService : ControllerBase, ICreditService
    {
        MessageModel responseMessage = new MessageModel();
        public async Task<ActionResult<MessageModel>> CreateCredit(ClaimsPrincipal currentUser, CreditRequestModel requestModel, IConfiguration _config, BankSystemContext dbContext)
        {
           
            
            string role = "";
            string username = requestModel.Username;
            Credits credit = requestModel.Credit;
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
                    if (dbContext.UserAccounts.Where(x => x.CreditId != null && x.UserUsername == username).Count() < 7)
                    {
                        if (ValidateUser(userAuthenticate) && ValidateCredit(credit))
                        {
                            UserAccounts userAccounts = new UserAccounts();
                            userAccounts.UserId = userAuthenticate.Id;
                            userAccounts.UserUsername = userAuthenticate.Username;
                            credit.Iban = IBANGenerator.GenerateIBANInVitoshaBank("Credit", dbContext);
                            credit.Interest = 6.9m;
                            credit.CreditAmount = CalculateInterest.CalculateCreditAmount(credit.Amount, period, credit.Interest);
                            credit.Instalment = CalculateInterest.CalculateInstalment(credit.CreditAmount, credit.Interest, period);
                            credit.CreditAmountLeft = credit.CreditAmount;
                            credit.PaymentDate = DateTime.Now.AddMonths(1);
                            await dbContext.AddAsync(credit);
                            await dbContext.SaveChangesAsync();
                            userAccounts.WalletId = dbContext.Wallets.FirstOrDefaultAsync(x => x.Iban == credit.Iban).Id;
                            await dbContext.AddAsync(userAccounts);
                            await dbContext.SaveChangesAsync();

                            SendEmail(userAuthenticate.Email, _config);
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

                responseMessage.Message = "User already has a wallet!";
                return StatusCode(400, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<CreditResponseModel>> GetCreditInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
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
                    foreach (var creditRef in dbContext.UserAccounts.Where(x => x.CreditId != null && x.UserUsername == username))
                    {
                        var credit = creditRef.Credit;
                        creditResponseModel.IBAN = credit.Iban;
                        creditResponseModel.Amount = Math.Round(credit.Amount, 2);
                        creditResponseModel.Instalment = credit.Instalment;
                        creditResponseModel.CreditAmount = credit.CreditAmount;

                        userCredits.UserCredits.Add(creditResponseModel);
                    }

                    if (userCredits.UserWallets.Count > 0)
                    {
                        return StatusCode(200, userCredits.UserCredits);
                    }

                    responseMessage.Message = "You don't have a Wallet!";
                    return StatusCode(400, responseMessage);
                }
            }
            return null;
        }
        public async Task<ActionResult<CreditResponseModel>> GetPayOffInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
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
                    foreach (var creditRef in dbContext.UserAccounts.Where(x => x.CreditId != null && x.UserUsername == username))
                    {
                        var credit = creditRef.Credit;
                        CreditPayOff payOff = new CreditPayOff();
                        await payOff.GetCreditPayOff(credit, username, dbContext);

                        if (credit.CreditAmountLeft == 0 && credit.CreditAmount > 0)
                        {
                            CreditRequestModel requestModel = new CreditRequestModel();
                            requestModel.Credit = credit;
                            requestModel.Username = username;
                            responseMessage.Message = "You have payed your Credit!";
                            await this.DeleteCredit(requestModel,currentUser, dbContext);
                        }
                        else
                        {
                            responseMessage.Message = "Successfully payed montly pay off!";
                            return StatusCode(200, responseMessage);
                        }
                    }
                }
                responseMessage.Message = "You don't have a Credit";
                return StatusCode(400, responseMessage);
            }

            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> SimulatePurchase(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext _context)
        {
            //amount credit product reciever
            var credit = requestModel.Credit;
            decimal amount = requestModel.Amount;
            var product = requestModel.Product;
            var reciever = requestModel.Reciever;
            var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            Credits creditExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    creditExists = await _context.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (creditExists != null)
                {
                    if (ValidateCreditAmount(amount, creditExists) && ValidateCredit(creditExists))
                    {
                        creditExists.Amount = creditExists.Amount - amount;
                        Transactions transaction = new Transactions();
                        transaction.SenderAccountInfo = creditExists.Iban;
                        transaction.RecieverAccountInfo = reciever;
                        //await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Purchasing {product}", _context, _messageModel);
                        await _context.SaveChangesAsync();
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
                else
                {
                    responseMessage.Message = "Credit not found";
                    return StatusCode(404, responseMessage);
                }
            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);

        }
        public async Task<ActionResult<MessageModel>> AddMoney(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext _context)
        {
            //credit amount
            var credit = requestModel.Credit;
            var amount = requestModel.Amount;
            var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            Credits creditsExists = null;
            ChargeAccounts bankAccounts = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    creditsExists = await _context.Credits.FirstOrDefaultAsync(x =>x.Iban == credit.Iban);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                if (creditsExists != null)
                {
                    bankAccounts = _context.ChargeAccounts.FirstOrDefault(x => x.Iban == bankAccounts.Iban);
                    return await ValidateDepositAmountAndCredit(userAuthenticate, creditsExists, currentUser, amount, bankAccounts, _context);
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
        public async Task<ActionResult<MessageModel>> Withdraw(CreditRequestModel requestModel, ClaimsPrincipal currentUser, string username, string reciever, BankSystemContext _context)
        {
            //credit amount
            var credit = requestModel.Credit;
            var amount = requestModel.Amount;
            var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            Credits creditExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    creditExists = await _context.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (creditExists != null)
                {
                    if (ValidateDepositAmountBankAccount(amount) && ValidateCredit(creditExists) && ValidateMinAmount(creditExists, amount))
                    {
                        creditExists.Amount = creditExists.Amount - amount;
                        Transactions transactions = new Transactions();
                        transactions.SenderAccountInfo = credit.Iban;
                        transactions.RecieverAccountInfo = reciever;
                        //await _transaction.CreateTransaction(userAuthenticate, currentUser, amount, transactions, $"Withdrawing {amount} leva", _context, _messageModel);
                        await _context.SaveChangesAsync();
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
                        responseMessage.Message = "Min amount is 10 lv!";
                        return StatusCode(406, responseMessage);
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
        public async Task<ActionResult<MessageModel>> DeleteCredit(CreditRequestModel requestModel, ClaimsPrincipal currentUser, BankSystemContext _context)
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
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                Credits creditsExists = null;

                if (user != null)
                {
                    creditsExists = await _context.Credits.FirstOrDefaultAsync(x => x.Iban == credit.Iban);
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

                _context.Credits.Remove(creditsExists);
                await _context.SaveChangesAsync();

                responseMessage.Message = "Credit deleted successfully!";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        private bool ValidateMinAmount(Credits credit, decimal amount)
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
        private bool ValidateCreditAmount(decimal amount, Credits credit)
        {
            if (credit.Amount < amount)
            {
                return false;
            }
            return true;
        }
        private bool ValidateCredit(Credits credits)
        {
            if (credits.Amount < 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUser(Users user)
        {
            if (user != null)
            {
                return true;
            }
            return false;
        }
        private async Task<ActionResult> ValidateDepositAmountAndCredit(Users userAuthenticate, Credits creditExists, ClaimsPrincipal currentUser, decimal amount, ChargeAccounts bankAccount, BankSystemContext _context)
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
                    Transactions transaction = new Transactions();
                    transaction.SenderAccountInfo = bankAccount.Iban;
                    transaction.RecieverAccountInfo = creditExists.Iban;
                    //await _transaction.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Depositing money in Credit Account", _context, _messageModel);
                    await _context.SaveChangesAsync();
                }
                else if (bankAccount.Amount < amount)
                {
                    responseMessage.Message = "You don't have enough money in bank account!";
                    return StatusCode(406, responseMessage);
                }
                else if (bankAccount == null)
                {
                    responseMessage.Message = "You don't have a bank account";
                    return StatusCode(400, responseMessage);
                }
            }
            responseMessage.Message = $"Succesfully deposited {amount} leva.";
            return StatusCode(200, responseMessage);
        }

        private void SendEmail(string email, IConfiguration _config)
        {
            var fromMail = new MailAddress(_config["Email:Email"], $"Credit account created");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
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
