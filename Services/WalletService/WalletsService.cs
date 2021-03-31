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
using VitoshaBank.Services.GenerateCardInfoService;
using VitoshaBank.Services.IbanGenereatorService;
using VitoshaBank.Services.WalletService.Interfaces;

namespace VitoshaBank.Services.WalletService
{
    public class WalletsService : ControllerBase, IWalletsService
    {
        private readonly BankSystemContext dbContext;
        private readonly IConfiguration _config;
        public WalletsService(BankSystemContext context, IConfiguration config)
        {
            dbContext = context;
            _config = config;
        }
        MessageModel responseMessage = new MessageModel();
        public async Task<ActionResult<ICollection<WalletResponseModel>>> GetWalletsInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                WalletResponseModel walletResponseModel = new WalletResponseModel();
                UserAccResponseModel userWallets = new UserAccResponseModel();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    foreach (var walletRef in dbContext.UserAccounts.Where(x => x.WalletId != null && x.UserUsername == username))
                    {
                        var wallet = walletRef.Wallet;
                        walletResponseModel.IBAN = wallet.Iban;
                        walletResponseModel.Amount = Math.Round(wallet.Amount, 2);
                        walletResponseModel.CardNumber = wallet.CardNumber;

                        if (walletResponseModel.CardNumber.StartsWith('5'))
                        {
                            walletResponseModel.CardBrand = "Master Card";
                        }
                        else
                        {
                            walletResponseModel.CardBrand = "Visa";
                        }

                        userWallets.UserWallets.Add(walletResponseModel);
                    }

                    if (userWallets.UserWallets.Count > 0)
                    {
                        return StatusCode(200, userWallets.UserWallets);
                    }

                    responseMessage.Message = "You don't have a Wallet!";
                    return StatusCode(400, responseMessage);

                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<MessageModel>> CreateWallet(ClaimsPrincipal currentUser, WalletRequestModel requestModel)
        {
            string role = "";
            var username = requestModel.Username;
            Wallet wallet = requestModel.Wallet;
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
                    if (dbContext.UserAccounts.Where(x => x.WalletId != null && x.UserUsername == username).Count() < 7)
                    {
                        if (ValidateUser(userAuthenticate) && ValidateWallet(wallet))
                        {
                            UserAccount userAccounts = new UserAccount();
                            userAccounts.UserId = userAuthenticate.Id;
                            userAccounts.UserUsername = userAuthenticate.Username;
                            

                            wallet.Iban = IBANGenerator.GenerateIBANInVitoshaBank("Wallet", dbContext);
                            wallet.CardNumber = GenerateCardInfo.GenerateNumber(11);
                            var CVV = GenerateCardInfo.GenerateCVV(3);
                            wallet.Cvv = _BCrypt.HashPassword(CVV);
                            wallet.CardExpirationDate = DateTime.Now.AddMonths(60);

                            await dbContext.AddAsync(wallet);
                            await dbContext.SaveChangesAsync();

                            userAccounts.WalletId = dbContext.Wallets.FirstOrDefaultAsync(x => x.Iban == wallet.Iban).Id;
                            await dbContext.AddAsync(userAccounts);
                            await dbContext.SaveChangesAsync();

                            SendEmail(userAuthenticate.Email, _config);
                            responseMessage.Message = "Wallet created succesfully!";
                            return StatusCode(200, responseMessage);
                        }
                        else if (ValidateUser(userAuthenticate) == false)
                        {
                            responseMessage.Message = "User not found!";
                            return StatusCode(404, responseMessage);
                        }
                        else if (ValidateWallet(wallet) == false)
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
        public async Task<ActionResult<MessageModel>> AddMoney(WalletRequestModel requestModel, ClaimsPrincipal currentUser, string username)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var amount = requestModel.Amount;
            Wallet wallet = requestModel.Wallet;
            Wallet walletExists = null;
            ChargeAccount chargeAccount = requestModel.ChargeAccount;
            ChargeAccount chargeAccountExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    walletExists = await dbContext.Wallets.FirstOrDefaultAsync(x => x.Iban == wallet.Iban);

                    if (walletExists != null)
                    {
                        chargeAccountExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAccount.Iban);

                        if (walletExists.CardExpirationDate < DateTime.Now)
                        {
                            responseMessage.Message = "Wallet Card is expired";
                            return StatusCode(406, responseMessage);
                        }

                        return await ValidateDepositAmountAndBankAccount(userAuthenticate, currentUser, walletExists, amount, chargeAccountExists);
                    }
                    else
                    {
                        responseMessage.Message = "Wallet not found";
                        return StatusCode(404, responseMessage);
                    }

                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
               
            }

            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> SimulatePurchase(WalletRequestModel requestModel, ClaimsPrincipal currentUser, string username /*ITransactionService _transation*/)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var product = requestModel.Product;
            var reciever = requestModel.Reciever;
            var amount = requestModel.Amount;
            Wallet wallet = requestModel.Wallet;
            Wallet walletExists = null;
            WalletResponseModel walletResponseModel = new WalletResponseModel();
            BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    walletExists = await dbContext.Wallets.FirstOrDefaultAsync(x => x.Iban == wallet.Iban);
                    if (walletExists != null && (wallet.CardNumber == walletExists.CardNumber && wallet.CardExpirationDate == walletExists.CardExpirationDate && _BCrypt.AuthenticateWalletCVV(wallet, walletExists)))
                    {
                        if (walletExists.CardExpirationDate < DateTime.Now)
                        {
                            responseMessage.Message = "Wallet Card is expired";
                            return StatusCode(406, responseMessage);
                        }

                        return await ValidatePurchaseAmountAndBankAccount(userAuthenticate, currentUser, walletExists, product, reciever, amount /*_transation*/ );
                    }
                    else
                    {
                        responseMessage.Message = "Wallet not found";
                        return StatusCode(404, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }

            }
            responseMessage.Message = "You are not autorized to do such actions!";
            return StatusCode(403, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> DeleteWallet(ClaimsPrincipal currentUser, WalletRequestModel requestModel)
        {
            string role = "";
            var username = requestModel.Username;
            Wallet wallet = requestModel.Wallet;
            Wallet walletExists = null;
            UserAccount userWallet = null;

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
                    walletExists = await dbContext.Wallets.FirstOrDefaultAsync(x => x.Iban == wallet.Iban);
                    userWallet = await dbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == walletExists.Id);
                }

                if (user == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else if (walletExists == null)
                {
                    responseMessage.Message = "User doesn't have a Wallet";
                    return StatusCode(400, responseMessage);
                }

                dbContext.Wallets.Remove(walletExists);
                await dbContext.SaveChangesAsync();
                dbContext.UserAccounts.Remove(userWallet);
                await dbContext.SaveChangesAsync();

                responseMessage.Message = $"Succsesfully deleted {user.Username} Wallet!";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        private bool ValidateWallet(Wallet wallet)
        {
            if (wallet.Amount < 0)
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
        private async Task<ActionResult> ValidateDepositAmountAndBankAccount(User userAuthenticate, ClaimsPrincipal currentUser, Wallet walletExists, decimal amount, ChargeAccount bankAccount /*ITransactionService _transation*/)
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
                    walletExists.Amount = walletExists.Amount + amount;
                    bankAccount.Amount = bankAccount.Amount - amount;
                    //Transactions transaction = new Transactions();
                    //transaction.SenderAccountInfo = bankAccount.Iban;
                   // transaction.RecieverAccountInfo = walletExists.Iban;

                   // await _transation.CreateTransaction(userAuthenticate, currentUser, amount, transaction, "Depositing money in Wallet", dbContext, responseMessage);
                    await dbContext.SaveChangesAsync();
                }
                else if (bankAccount.Amount < amount)
                {
                    responseMessage.Message = "You don't have enough money in Bank Account!";
                    return StatusCode(406, responseMessage);
                }
                else if (bankAccount == null)
                {
                    responseMessage.Message = "You don't have a bank account";
                    return StatusCode(400, responseMessage);
                }
            }
            responseMessage.Message = $"Succesfully deposited {amount} leva in Wallet.";
            return StatusCode(200, responseMessage);
        }
        private async Task<ActionResult> ValidatePurchaseAmountAndBankAccount(User userAuthenticate, ClaimsPrincipal currentUser, Wallet walletExists, string product, string reciever, decimal amount /*ITransactionService _transation*/)
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
                if (walletExists.Amount < amount)
                {
                    responseMessage.Message = "You don't have enough money in wallet!";
                    return StatusCode(406, responseMessage);
                }
                else
                {
                    //Transactions transaction = new Transactions();
                    walletExists.Amount = walletExists.Amount - amount;
                   // transaction.SenderAccountInfo = walletExists.Iban;
                   // transaction.RecieverAccountInfo = reciever;
                   // await _transation.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Purchasing {product} with Wallet", dbContext, responseMessage);
                    await dbContext.SaveChangesAsync();
                }
            }

            responseMessage.Message = $"Succesfully purhcased {product}.";
            return StatusCode(200, responseMessage);
        }
        private void SendEmail(string email, IConfiguration _config)
        {
            var fromMail = new MailAddress(_config["Email:Email"], $"Wallet created");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
            string subject = "Your wallet is successfully created";
            string body = "<br/><br/>We are excited to tell you that your wallet was created succesfully. You can use it instanstly.";

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
