﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.RequestModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.BcryptHasherService;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.DebitCardService.Interfaces;
using VitoshaBank.Services.GenerateCardInfoService;

namespace VitoshaBank.Services.DebitCardService
{
    public class DebitCardsService : ControllerBase, IDebitCardsService
    {
        private readonly BankSystemContext dbContext;
        private readonly IConfiguration _config;
        public DebitCardsService(BankSystemContext context, IConfiguration config)
        {
            dbContext = context;
            _config = config;
        }
        BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();
        MessageModel responseMessage = new MessageModel();

        public async Task<ActionResult<MessageModel>> CreateDebitCard(ClaimsPrincipal currentUser, string username, ChargeAccount bankAccount, Card card)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                Card cardExists = null;
                ChargeAccount bankAccountExists = null;

                if (userAuthenticate != null)
                {
                    bankAccountExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == bankAccount.Iban);
                    cardExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == bankAccountExists.Id);
                }


                if (cardExists == null && bankAccountExists != null)
                {
                    if (ValidateUser(userAuthenticate))
                    {
                       
                        card.ChargeAccountId = bankAccountExists.Id;
                        card.CardNumber = GenerateCardInfo.GenerateNumber(11);
                        var CVV = GenerateCardInfo.GenerateCVV(3);
                        card.Cvv = _BCrypt.HashPassword(CVV);
                        card.CardExpirationDate = DateTime.Now.AddMonths(60);
                        dbContext.Add(card);
                        await dbContext.SaveChangesAsync();
                        responseMessage.Message = "Debit Card created succesfully!";
                        return StatusCode(200, responseMessage);
                    }
                    else if (ValidateUser(userAuthenticate) == false)
                    {
                        responseMessage.Message = "User not found!";
                        return StatusCode(404, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "No Bank Account found";
                    return StatusCode(404, responseMessage);
                }

                responseMessage.Message = "User already has a Debit Card!";
                return StatusCode(400, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<DebitCardResponseModel>> GetDebitCardInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                ChargeAccount bankAccountExits = null;
                Card debitCardExists = null;
                DebitCardResponseModel debitCardResponseModel = new DebitCardResponseModel();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    bankAccountExits = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    debitCardExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == bankAccountExits.Id);
                }

                if (debitCardExists != null)
                {
                    debitCardResponseModel.CardNumber = debitCardExists.CardNumber;
                    if (debitCardResponseModel.CardNumber.StartsWith('5'))
                    {
                        debitCardResponseModel.CardBrand = "Master Card";
                    }
                    else
                    {
                        debitCardResponseModel.CardBrand = "Visa";
                    }

                    return StatusCode(200, debitCardResponseModel);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
            responseMessage.Message = "You don't have a Debit Card!!";
            return StatusCode(400, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> AddMoney(string cardNumber, string CVV, DateTime expireDate, ClaimsPrincipal currentUser, string username, decimal amount/*, ITransactionService _transactionService*/, IChargeAccountsService _chargeAccService)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            ChargeAccount bankAccountsExists = null;
            Card cardsExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    bankAccountsExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    cardsExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == bankAccountsExists.Id);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (bankAccountsExists != null && cardsExists != null && (cardNumber == cardsExists.CardNumber && expireDate == cardsExists.CardExpirationDate && _BCrypt.AuthenticateDebitCardCVV(CVV, cardsExists)))
                {
                    if (cardsExists.CardExpirationDate < DateTime.Now)
                    {
                        responseMessage.Message = "Debit Card is expired";
                        return StatusCode(406, responseMessage);
                    }
                    ChargeAccountRequestModel requestModel = new ChargeAccountRequestModel();
                    requestModel.ChargeAccount = bankAccountsExists;
                    requestModel.Amount = amount;
                    await _chargeAccService.AddMoney(requestModel, currentUser, username /*_transactionService*/);
                }
                else if (bankAccountsExists == null)
                {
                    responseMessage.Message = "Bank Account not found";
                    return StatusCode(404, responseMessage);
                }
                else if (cardsExists == null)
                {
                    responseMessage.Message = "Debit Card not found";
                    return StatusCode(404, responseMessage);
                }

            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> SimulatePurchase(string cardNumber, string CVV, DateTime expireDate, string product, ClaimsPrincipal currentUser, string username, decimal amount, string reciever/*, ITransactionService _transactionService*/, IChargeAccountsService _chargeAccService)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            ChargeAccount bankAccountsExists = null;
            Card cardsExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    bankAccountsExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    cardsExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == bankAccountsExists.Id);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (bankAccountsExists != null && cardsExists != null && (cardNumber == cardsExists.CardNumber && expireDate == cardsExists.CardExpirationDate && _BCrypt.AuthenticateDebitCardCVV(CVV, cardsExists)))
                {
                    if (cardsExists.CardExpirationDate < DateTime.Now)
                    {
                        responseMessage.Message = "Debit Card is expired";
                        return StatusCode(406, responseMessage);
                    }
                    ChargeAccountRequestModel requestModel = new ChargeAccountRequestModel();
                    requestModel.ChargeAccount = bankAccountsExists;
                    requestModel.Product = product;
                    requestModel.Amount = amount;
                    requestModel.Reciever = reciever;
                    await _chargeAccService.SimulatePurchase(requestModel, currentUser, username/*, _transactionService*/);
                }
                else if (bankAccountsExists == null)
                {
                    responseMessage.Message = "Bank Account not found";
                    return StatusCode(404, responseMessage);
                }
                else if (cardsExists == null)
                {
                    responseMessage.Message = "Debit Card not found";
                    return StatusCode(404, responseMessage);
                }
            }

            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> Withdraw(string cardNumber, string CVV, DateTime expireDate, ClaimsPrincipal currentUser, string username, decimal amount, string reciever, /*ITransactionService _transactionService*/IChargeAccountsService _chargeAccService)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            ChargeAccount bankAccountsExists = null;
            Card cardsExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    bankAccountsExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    cardsExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == bankAccountsExists.Id && x.CardNumber == cardNumber && x.Cvv == CVV && x.CardExpirationDate == expireDate);
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

                if (bankAccountsExists != null && cardsExists != null && (cardNumber == cardsExists.CardNumber && expireDate == cardsExists.CardExpirationDate && _BCrypt.AuthenticateDebitCardCVV(CVV, cardsExists)))
                {
                    if (cardsExists.CardExpirationDate < DateTime.Now)
                    {
                        responseMessage.Message = "Debit Card is expired";
                        return StatusCode(406, responseMessage);
                    }
                    ChargeAccountRequestModel requestModel = new ChargeAccountRequestModel();
                    requestModel.ChargeAccount = bankAccountsExists;
                    
                    requestModel.Amount = amount;
                    requestModel.Reciever = reciever;
                    await _chargeAccService.Withdraw(requestModel, currentUser, username/*_transactionService*/);
                }
                else if (bankAccountsExists == null)
                {
                    responseMessage.Message = "Bank Account not found";
                    return StatusCode(404, responseMessage);
                }
                else if (cardsExists == null)
                {
                    responseMessage.Message = "Debit Card not found";
                    return StatusCode(404, responseMessage);
                }
            }

            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> DeleteDebitCard(ClaimsPrincipal currentUser,DebitCardRequestModel requestModel ,string username)
        {
            string role = "";
            
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                Card cardExists = null;
                

                if (user != null)
                {
                    cardExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.Id == requestModel.Card.Id);
                }

                if (user == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else if (cardExists == null)
                {
                    responseMessage.Message = "User doesn't have a Debit Card!";
                    return StatusCode(400, responseMessage);
                }

                dbContext.Cards.Remove(cardExists);
                await dbContext.SaveChangesAsync();

                responseMessage.Message = $"Succsesfully deleted {user.Username} Debit Card!";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }

        private bool ValidateUser(User user)
        {
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
