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
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.DividentService;
using VitoshaBank.Services.IbanGenereatorService;

namespace VitoshaBank.Services.DepositService
{
    public class DepositsService : ControllerBase, IDepositsService
    {

        MessageModel responseMessage = new MessageModel();
        public async Task<ActionResult<ICollection<DepositResponseModel>>> GetDepositsInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                DepositResponseModel depositResponseModel = new DepositResponseModel();
                UserAccResponseModel userDeposits = new UserAccResponseModel();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    foreach (var depositRef in dbContext.UserAccounts.Where(x => x.DepositId != null && x.UserUsername == username))
                    {
                        var deposit = depositRef.Deposit;
                        depositResponseModel.Divident = deposit.Divident;
                        depositResponseModel.PaymentDate = deposit.PaymentDate;
                        userDeposits.UserDeposits.Add(depositResponseModel);
                    }
                    if (userDeposits.UserDeposits.Count > 0)
                    {
                        return StatusCode(200, userDeposits.UserDeposits);
                    }
                    responseMessage.Message = "You don't have a Deposit!";
                    return StatusCode(400, responseMessage);

                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }

        }
        public async Task<ActionResult<DepositResponseModel>> CheckDividentsPayments(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                DepositResponseModel depositResponseModel = new DepositResponseModel();
                DividentValidation dividentService = new DividentValidation();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    bool isChecked = false;
                    foreach (var depositRef in dbContext.UserAccounts.Where(x => x.DepositId != null && x.UserUsername == username))
                    {
                        await dividentService.GetDividentPayment(depositRef.Deposit);
                        isChecked = true;
                    }
                    if (isChecked)
                    {
                        responseMessage.Message = "Check susscessfull";
                        return StatusCode(200, responseMessage);
                    }
                    else
                    {
                        responseMessage.Message = "You don't have a Deposit!";
                        return StatusCode(400, responseMessage);
                    }
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }


        }
        public async Task<ActionResult<DepositResponseModel>> GetDividentsInfo(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                DepositResponseModel depositResponseModel = new DepositResponseModel();
                UserAccResponseModel userDeposits = new UserAccResponseModel();
                DividentValidation dividentService = new DividentValidation();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    bool isChecked = false;
                    foreach (var depositRef in dbContext.UserAccounts.Where(x => x.DepositId != null && x.UserUsername == username))
                    {
                        await dividentService.GetDividentPayment(depositRef.Deposit);
                        isChecked = true;

                    }
                    if (isChecked)
                    {
                        responseMessage.Message = "Check susscessfull";
                        return StatusCode(200, depositResponseModel);
                    }
                    else
                    {
                        responseMessage.Message = "You don't have a Deposit!";
                        return StatusCode(400, responseMessage);
                    }
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<MessageModel>> CreateDeposit(ClaimsPrincipal currentUser, DepositRequestModel requestModel, IConfiguration config, BankSystemContext dbContext)
        {
            string role = "";
            var username = requestModel.Username;
            Deposits deposit = requestModel.Deposit;

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
                    if (dbContext.UserAccounts.Where(x => x.DepositId != null && x.UserUsername == username).Count() < 6)
                    {
                        if (ValidateUser(userAuthenticate) && ValidateDeposits(deposit))
                        {
                            UserAccounts userAccounts = new UserAccounts();
                            userAccounts.UserId = userAuthenticate.Id;
                            userAccounts.UserUsername = userAuthenticate.Username;
                           
                            deposit.Iban = IBANGenerator.GenerateIBANInVitoshaBank("Deposit", dbContext);

                            if (deposit.TermOfPayment == 3 || deposit.TermOfPayment == 6 || deposit.TermOfPayment == 12 || deposit.TermOfPayment == 1)
                            {
                                deposit.PaymentDate = DateTime.Now.AddMonths(deposit.TermOfPayment);
                                deposit.Divident = CalculateDivident.GetDividentPercent(deposit.Amount, deposit.TermOfPayment);
                                await dbContext.AddAsync(deposit);
                                await dbContext.SaveChangesAsync();

                                userAccounts.WalletId = dbContext.Deposits.FirstOrDefaultAsync(x => x.Iban == deposit.Iban).Id;
                                await dbContext.AddAsync(userAccounts);
                                await dbContext.SaveChangesAsync();

                                SendEmail(userAuthenticate.Email, config);
                                responseMessage.Message = "Deposit created succesfully";
                                return StatusCode(200, responseMessage);
                            }
                            else
                            {
                                responseMessage.Message = "Deposit Term of paymet must be 1, 3, 6 or 12 months";
                                return StatusCode(400, responseMessage);
                            }
                        }
                        else if (ValidateUser(userAuthenticate) == false)
                        {
                            responseMessage.Message = "User not found";
                            return StatusCode(404, responseMessage);
                        }
                        else if (ValidateDeposits(deposit) == false)
                        {
                            responseMessage.Message = "Invalid payment amount!";
                            return StatusCode(400, responseMessage);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        responseMessage.Message = "User already has the maximum of 5 Deposits!";
                        return StatusCode(400, responseMessage);
                    }

                }
                else
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<MessageModel>> AddMoney(DepositRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var amount = requestModel.Amount;
            Deposits deposit = requestModel.Deposit;
            Deposits depositExists = null;
            ChargeAccounts chargeAccount = requestModel.ChargeAccount;
            ChargeAccounts chargeAccountExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    depositExists = await dbContext.Deposits.FirstOrDefaultAsync(x => x.Iban == deposit.Iban);
                    if (depositExists != null)
                    {
                        chargeAccountExists = await dbContext.ChargeAccounts.FirstOrDefaultAsync(x => x.Iban == chargeAccount.Iban);
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
                            if (chargeAccount != null && chargeAccount.Amount > amount)
                            {
                                deposit.Amount = deposit.Amount + amount;
                                deposit.PaymentDate = DateTime.Now.AddMonths(6);
                                chargeAccount.Amount = chargeAccount.Amount - amount;
                                //Transactions transaction = new Transactions();
                                //transaction.SenderAccountInfo = $"User {userAuthenticate.FirstName} {userAuthenticate.LastName}";
                                //transaction.RecieverAccountInfo = depositsExists.Iban;
                                await dbContext.SaveChangesAsync();
                                //await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, "Added money - Bank Account - Deposit account", _context, _messageModel);
                            }
                            else if (chargeAccount.Amount < amount)
                            {
                                responseMessage.Message = "You don't have enough money in Bank Account!";
                                return StatusCode(406, responseMessage);
                            }
                            else if (chargeAccount == null)
                            {
                                responseMessage.Message = "You don't have a Bank Account";
                                return StatusCode(400, responseMessage);
                            }
                        }
                        responseMessage.Message = $"Succesfully deposited {amount} leva.";
                        return StatusCode(200, responseMessage);
                    }
                    else
                    {
                        responseMessage.Message = "Deposit not found";
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
        public async Task<ActionResult<MessageModel>> WithdrawMoney(DepositRequestModel requestModel, ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var amount = requestModel.Amount;
            Deposits deposit = requestModel.Deposit;
            Deposits depositExists = null;
            DepositResponseModel depositResponseModel = new DepositResponseModel();


            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    depositExists = await dbContext.Deposits.FirstOrDefaultAsync(x => x.Iban == deposit.Iban);
                    if (depositExists != null)
                    {
                        depositExists.Amount = depositExists.Amount - amount;
                        depositExists.PaymentDate = DateTime.Now.AddMonths(depositExists.TermOfPayment);
                        await dbContext.SaveChangesAsync();

                        //Transactions transaction = new Transactions();
                        //transaction.SenderAccountInfo = depositsExists.Iban;
                        //transaction.RecieverAccountInfo = $"{userAuthenticate.FirstName} {userAuthenticate.LastName}";
                        //await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Withdrawing {amount}", _context, _messageModel);
                        responseMessage.Message = "Money withdrawed successfully!";
                        return StatusCode(200, responseMessage);
                    }
                    else
                    {
                        responseMessage.Message = "Deposit not found";
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
        public async Task<ActionResult<MessageModel>> DeleteDeposit(ClaimsPrincipal currentUser, DepositRequestModel requestModel, BankSystemContext dbContext)
        {
            string role = "";
            var username = requestModel.Username;
            Deposits deposit = requestModel.Deposit;
            Deposits depositExists = null;
            UserAccounts userDeposit = null;

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
                    depositExists = await dbContext.Deposits.FirstOrDefaultAsync(x => x.Iban == deposit.Iban);
                    userDeposit = await dbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == depositExists.Id);
                }

                if (user == null)
                {
                    responseMessage.Message = "User not found!";
                    return StatusCode(404, responseMessage);
                }
                else if (depositExists == null)
                {
                    responseMessage.Message = "User doesn't have a Deposit";
                    return StatusCode(400, responseMessage);
                }

                dbContext.Deposits.Remove(depositExists);
                dbContext.UserAccounts.Remove(userDeposit);
                await dbContext.SaveChangesAsync();

                responseMessage.Message = $"Succsesfully deleted {user.Username} Deposit!";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        private bool ValidateDeposits(Deposits deposit)
        {
            if (deposit.Amount < 0)
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
        private void SendEmail(string email, IConfiguration _config)
        {
            var fromMail = new MailAddress(_config["Email:Email"], $"Deposit account created");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
            string subject = "Your deposit account is successfully created";
            string body = "<br/><br/>We are excited to tell you that your deposit account is created succesfully. You can use it instantly.";

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
