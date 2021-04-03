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
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.DebitCardService.Interfaces;
using VitoshaBank.Services.IbanGenereatorService;

namespace VitoshaBank.Services.ChargeAccountService
{
    public class ChargeAccountsService : ControllerBase, IChargeAccountsService
    {
        private readonly BankSystemContext dbContext;
        private readonly IConfiguration _config;
        public ChargeAccountsService(BankSystemContext context, IConfiguration config)
        {
            dbContext = context;
            _config = config;
        }
        BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();
        MessageModel responseModel = new MessageModel();
        
        public async Task<ActionResult<MessageModel>> CreateChargeAccount(ClaimsPrincipal currentUser, ChargeAccountRequestModel requestModel, IDebitCardsService _debitCardService)
        {
            string role = "";
            var username = requestModel.Username;
            ChargeAccount chargeAcc = requestModel.ChargeAccount;
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
                    if (dbContext.UserAccounts.Where(x => x.ChargeAccountId != null && x.UserUsername == username).Count() < 10)
                    {
                        if (ValidateUser(userAuthenticate) && ValidateChargeAccount(chargeAcc))
                        {
                            UserAccount userAccounts = new UserAccount();
                            userAccounts.UserId = userAuthenticate.Id;
                            userAccounts.UserUsername = userAuthenticate.Username;

                            chargeAcc.Iban = IBANGenerator.GenerateIBANInVitoshaBank("ChargeAccount", dbContext);
                            await dbContext.AddAsync(chargeAcc);
                            await dbContext.SaveChangesAsync();

                            userAccounts.ChargeAccountId = chargeAcc.Id;
                            await dbContext.AddAsync(userAccounts);
                            await dbContext.SaveChangesAsync();

                            Card card = new Card();
                            DebitCardRequestModel debitCardRequest = new DebitCardRequestModel();
                            debitCardRequest.Card = card;
                            await _debitCardService.CreateDebitCard(currentUser, username, chargeAcc, card);

                            SendEmail(userAuthenticate.Email, _config);
                            responseModel.Message = "Charge Account created succesfully";
                            return StatusCode(201, responseModel);
                        }
                        else if (ValidateUser(userAuthenticate) == false)
                        {
                            responseModel.Message = "User not found!";
                            return StatusCode(404, responseModel);
                        }
                        else if (ValidateChargeAccount(chargeAcc) == false)
                        {
                            responseModel.Message = "Invalid parameteres!";
                            return StatusCode(400, responseModel);
                        }
                    }

                }

                responseModel.Message = "User already has a Charge Account!";
                return StatusCode(400, responseModel);
            }
            else
            {
                responseModel.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseModel);
            }
        }
        public async Task<ActionResult<ICollection<ChargeAccountResponseModel>>> GetBankAccountInfo(ClaimsPrincipal currentUser, string username )
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                ChargeAccountResponseModel chargeAccResponseModel = new ChargeAccountResponseModel();
                UserAccResponseModel userChargeAccounts = new UserAccResponseModel();

                if (userAuthenticate == null)
                {
                    responseModel.Message = "User not found";
                    return StatusCode(404, responseModel);
                }
                else
                {
                    foreach (var chargeAccRef in dbContext.UserAccounts.Where(x => x.ChargeAccountId != null && x.UserUsername == username))
                    {
                        var chargeAcc = chargeAccRef.ChargeAccount;
                        chargeAccResponseModel.IBAN = chargeAcc.Iban;
                        chargeAccResponseModel.Amount = Math.Round(chargeAcc.Amount, 2);

                        userChargeAccounts.UserChargeAcc.Add(chargeAccResponseModel);
                    }

                    if (userChargeAccounts.UserWallets.Count > 0)
                    {
                        return StatusCode(200, userChargeAccounts.UserWallets);
                    }

                    responseModel.Message = "You don't have a Charge Account!";
                    return StatusCode(400, responseModel);
                }
            }
            else
            {
                responseModel.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseModel);
            }
        }
        public async Task<ActionResult<MessageModel>> DepositMoney(ChargeAccount bankAccount,Deposit deposit, ClaimsPrincipal currentUser, string username, decimal amount /*ITransactionService _transactionService*/)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            ChargeAccount bankAccounts = null;
            Deposit depositsExist = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    bankAccounts = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == bankAccount.Iban);
                    depositsExist = await dbContext.Deposits.FirstOrDefaultAsync(x => x.Iban == deposit.Iban);
                }
                else
                {
                    responseModel.Message = "User not found!";
                    return StatusCode(404, responseModel);
                }

                if (bankAccounts != null && depositsExist != null)
                {
                    if (ValidateDepositAmountChargeAccount(amount))
                    {
                        bankAccounts.Amount = bankAccounts.Amount + amount;
                        depositsExist.Amount = depositsExist.Amount - amount;
                        await dbContext.SaveChangesAsync();
                        Transaction transactions = new Transaction();
                        transactions.RecieverAccountInfo = bankAccounts.Iban;
                        transactions.SenderAccountInfo = depositsExist.Iban;
                      //  await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transactions, "Depositing money Deposit Account -> Charge Account", _context, messageModel);
                        responseModel.Message = "Money deposited succesfully!";
                        return StatusCode(200, responseModel);
                    }
                    responseModel.Message = "Invalid deposit amount!";
                    return StatusCode(400, responseModel);
                }
                else
                {
                    responseModel.Message = "Charge Account not found";
                    return StatusCode(404, responseModel);
                }
            }
            responseModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseModel);
        }
        public async Task<ActionResult<MessageModel>> SimulatePurchase(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username  /*ITransactionService _transation*/)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var product = requestModel.Product;
            var reciever = requestModel.Reciever;
            var amount = requestModel.Amount;
            ChargeAccount chargeAcc = requestModel.ChargeAccount;
            ChargeAccount chargeAccExists = null;
            ChargeAccountResponseModel walletResponseModel = new ChargeAccountResponseModel();
            BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    chargeAccExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAcc.Iban);

                    if (chargeAccExists != null && ValidateDepositAmountChargeAccount(amount) && ValidateChargeAccount(chargeAcc, amount))
                    {
                        chargeAcc.Amount = chargeAcc.Amount - amount;
                        Transaction transactions = new Transaction();
                        transactions.SenderAccountInfo = chargeAccExists.Iban;
                        transactions.RecieverAccountInfo = reciever;
                        // await _transaction.CreateTransaction(userAuthenticate, currentUser, amount, transactions, $"Purchasing {product} with Charge Account", dbContext, _messageModel);
                        await dbContext.SaveChangesAsync();
                        responseModel.Message = $"Succesfully purhcased {product}.";
                        return StatusCode(200, responseModel);
                    }
                    else if (ValidateDepositAmountChargeAccount(amount) == false)
                    {
                        responseModel.Message = "Invalid payment amount!";
                        return StatusCode(400, responseModel);
                    }
                    else if (ValidateChargeAccount(chargeAcc, amount) == false)
                    {
                        responseModel.Message = "You don't have enough money in Charge account!";
                        return StatusCode(406, responseModel);
                    }
                }
                else
                {
                    responseModel.Message = "Charge Account not found";
                    return StatusCode(404, responseModel);
                }
            }

            responseModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseModel);
        }
        public async Task<ActionResult<MessageModel>> AddMoney(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var amount = requestModel.Amount;
            ChargeAccount chargeAcc = requestModel.ChargeAccount;
            ChargeAccount chargeAccExists = null;

            string role = "";
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                if (userAuthenticate != null)
                {
                    chargeAccExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAcc.Iban);

                    if (chargeAccExists != null)
                    {
                        if (ValidateDepositAmountChargeAccount(amount))
                        {
                            chargeAcc.Amount = chargeAcc.Amount + amount;
                            await dbContext.SaveChangesAsync();
                            Transaction transactions = new Transaction();
                            transactions.SenderAccountInfo = $"User {userAuthenticate.FirstName} {userAuthenticate.LastName}";
                            transactions.RecieverAccountInfo = chargeAcc.Iban;
                            //await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transactions, "Depositing money in Charge Account", dbContext, messageModel);
                            responseModel.Message = $"Succesfully deposited {amount} leva in Charge Account.";
                            return StatusCode(200, responseModel);
                        }

                        responseModel.Message = "Invalid money amount!";
                        return StatusCode(400, responseModel);
                    }
                    else
                    {
                        responseModel.Message = "Charge Account not found";
                        return StatusCode(404, responseModel);
                    }

                }
                else
                {
                    responseModel.Message = "User not found!";
                    return StatusCode(404, responseModel);
                }


            }

            responseModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseModel);
        }
        public async Task<ActionResult<MessageModel>> Withdraw(ChargeAccountRequestModel requestModel, ClaimsPrincipal currentUser, string username )
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var amount = requestModel.Amount;
            ChargeAccount chargeAcc = requestModel.ChargeAccount;
            ChargeAccount chargeAccExists = null;
            ChargeAccountResponseModel chargeAccResponseModel = new ChargeAccountResponseModel();

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    chargeAccExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAcc.Iban);

                    if (chargeAccExists != null)
                    {
                        if (ValidateDepositAmountChargeAccount(amount) && ValidateChargeAccount(chargeAcc, amount) && ValidateMinAmount(chargeAcc, amount))
                        {
                            chargeAcc.Amount = chargeAcc.Amount - amount;
                            Transaction transactions = new Transaction();
                            transactions.SenderAccountInfo = chargeAcc.Iban;
                            transactions.RecieverAccountInfo = $"{userAuthenticate.FirstName} {userAuthenticate.LastName}";
                            // await _transaction.CreateTransaction(userAuthenticate, currentUser, amount, transactions, $"Withdrawing {amount} leva", dbContext, messageModel);
                            await dbContext.SaveChangesAsync();
                            responseModel.Message = $"Succesfully withdrawed {amount} leva.";
                            return StatusCode(200, responseModel);
                        }
                        else if (ValidateDepositAmountChargeAccount(amount) == false)
                        {
                            responseModel.Message = "Invalid payment amount!";
                            return StatusCode(400, responseModel);
                        }
                        else if (ValidateChargeAccount(chargeAcc, amount) == false)
                        {
                            responseModel.Message = "You don't have enough money in Charge Account!";
                            return StatusCode(406, responseModel);
                        }
                        else if (ValidateMinAmount(chargeAcc, amount) == false)
                        {
                            responseModel.Message = "Min amount is 10 leva!";
                            return StatusCode(406, responseModel);
                        }

                    }
                    else
                    {
                        responseModel.Message = "Charge Account not found";
                        return StatusCode(404, responseModel);
                    }

                }
                else
                {
                    responseModel.Message = "User not found!";
                    return StatusCode(404, responseModel);
                }

            }
            responseModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseModel);
        }
        public async Task<ActionResult<MessageModel>> DeleteBankAccount(ClaimsPrincipal currentUser, ChargeAccountRequestModel requestModel)
        {
            string role = "";
            var username = requestModel.Username;
            ChargeAccount chargeAcc = requestModel.ChargeAccount;
            ChargeAccount chargeAccExists = null;
            Card cardExists = null;
            Credit creditExists = null;
            UserAccount userChargeAcc = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (user != null)
                {
                    chargeAccExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAcc.Iban);
                    cardExists = await dbContext.Cards.FirstOrDefaultAsync(x => x.ChargeAccountId == chargeAccExists.Id);
                    creditExists = await dbContext.Credits.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    userChargeAcc = await dbContext.UserAccounts.FirstOrDefaultAsync(x => x.ChargeAccountId == chargeAccExists.Id);

                }

                if (user == null)
                {
                    responseModel.Message = "User not found";
                    return StatusCode(404, responseModel);
                }

                if (chargeAccExists == null)
                {
                    responseModel.Message = "User doesn't have a Charge Account";
                    return StatusCode(400, responseModel);
                }

                if (cardExists != null)
                {
                    dbContext.Cards.Remove(cardExists);
                    await dbContext.SaveChangesAsync();
                }

                if (creditExists != null)
                {
                    responseModel.Message = "You can't delete Charge account if you have an existing credit!";
                    return StatusCode(406, responseModel);
                }

                dbContext.ChargeAccounts.Remove(chargeAccExists);
                await dbContext.SaveChangesAsync();
                dbContext.UserAccounts.Remove(userChargeAcc);
                await dbContext.SaveChangesAsync();

                responseModel.Message = $"Succsesfully deleted {user.Username} Charge Account and Debit Card!";
                return StatusCode(200, responseModel);

            }
            else
            {
                responseModel.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseModel);
            }
        }
        private bool ValidateChargeAccount(ChargeAccount chargeAccounts)
        {
            if (chargeAccounts.Amount < 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateDepositAmountChargeAccount(decimal amount)
        {
            if (amount > 0)
            {
                return true;
            }

            return false;
        }
        private bool ValidateMinAmount(ChargeAccount chargeAcc, decimal amount)
        {
            if (amount >= 10 && amount <= chargeAcc.Amount)
            {
                return true;
            }

            return false;
        }
        private bool ValidateChargeAccount(ChargeAccount chargeAcc, decimal amount)
        {
            if (chargeAcc != null && chargeAcc.Amount > amount)
            {
                return true;
            }
            else
            {
                return false;
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

        private void SendEmail(string email, IConfiguration _config)
        {
            var fromMail = new MailAddress(_config["Email:Email"], $"Charge Account created");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
            string subject = "Your charge account and debit card are successfully created";
            string body = "<br/><br/>We are excited to tell you that your charge account and debit card were created succesfully. You can use it instantly.";

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
