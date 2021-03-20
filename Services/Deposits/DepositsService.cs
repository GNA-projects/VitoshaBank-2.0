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
using VitoshaBank.Data.ResponseModels;

namespace VitoshaBank.Services.DepositService
{
    public class DepositsService:ControllerBase
    {
        BankSystemContext _context = new BankSystemContext();
        MessageModel responseMessage = new MessageModel();
        public async Task<ActionResult<DepositResponseModel>> GetDepositInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                UserAccounts depositIdExists = null;
                Deposits depositExists = null;
                DepositResponseModel depositResponseModel = new DepositResponseModel();

                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    depositIdExists = await _context.Useraccounts.FirstOrDefaultAsync(x => x.UserId== userAuthenticate.Id);
                    depositExists = await _context.Deposits.FirstOrDefaultAsync(x => x.Id == depositIdExists.DepositId);
                }

                if (depositExists != null)
                {
                    depositResponseModel.IBAN = depositExists.Iban;
                    depositResponseModel.Amount = Math.Round(depositExists.Amount, 2);
                    depositResponseModel.PaymentDate = depositExists.PaymentDate;

                    return StatusCode(200, depositResponseModel);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }

            responseMessage.Message = "You don't have a Deposit!";
            return StatusCode(400, responseMessage);
        }
        public async Task<ActionResult<DepositResponseModel>> GetDividentInfo(ClaimsPrincipal currentUser, string username, BankSystemContext _context, IDividentPaymentService _dividentPayment, MessageModel _messageModel)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                Deposits depositExists = null;
                UserAccounts depositIdExists = null;
                DepositResponseModel depositResponseModel = new DepositResponseModel();

                if (userAuthenticate == null)
                {
                    _messageModel.Message = "User not found";
                    return StatusCode(404, _messageModel);
                }
                else
                {
                    depositIdExists = await _context.Useraccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    depositExists = await _context.Deposits.FirstOrDefaultAsync(x => x.Id == depositIdExists.DepositId);
                }

                if (depositExists != null)
                {
                    await _dividentPayment.GetDividentPayment(depositExists, _messageModel, _context);
                    _messageModel.Message = "Check susscessfull";
                    return StatusCode(200, depositResponseModel);
                }
            }
            else
            {
                _messageModel.Message = "You are not authorized to do such actions";
                return StatusCode(403, _messageModel);
            }

            _messageModel.Message = "You don't have a Deposit!";
            return StatusCode(400, _messageModel);
        }
        public async Task<ActionResult<MessageModel>> CreateDeposit(ClaimsPrincipal currentUser, string username, Deposits deposits, IIBANGeneratorService _IBAN, IConfiguration _config)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                Deposits depositExists = null;
                UserAccounts depositIdExists = null;

                if (userAuthenticate != null)
                {
                    depositIdExists = await _context.Useraccounts.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id);
                    depositExists = await _context.Deposits.FirstOrDefaultAsync(x => x.Id == depositIdExists.DepositId);
                }
                else
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, re);
                }


                if (depositExists == null)
                {
                    if (ValidateUser(userAuthenticate) && ValidateDeposits(deposits))
                    {
                        deposits.UserId = userAuthenticate.Id;
                        deposits.Iban = _IBAN.GenerateIBANInVitoshaBank("Deposit", _context);
                        if (deposits.TermOfPayment == 3 || deposits.TermOfPayment == 6 || deposits.TermOfPayment == 12 || deposits.TermOfPayment == 1)
                        {
                            deposits.PaymentDate = DateTime.Now.AddMonths(deposits.TermOfPayment);

                            deposits.Divident = CalculateDivident.GetDividentPercent(deposits.Amount, deposits.TermOfPayment);
                            _context.Add(deposits);
                            await _context.SaveChangesAsync();

                            SendEmail(userAuthenticate.Email, _config);

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
                    else if (ValidateDeposits(deposits) == false)
                    {
                        responseMessage.Message = "Invalid payment amount!";
                        return StatusCode(400, responseMessage);
                    }
                }

                responseMessage.Message = "User already has Deposit!";
                return StatusCode(400, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<MessageModel>> AddMoney(Deposits deposit, ChargeAccounts bankAccount, ClaimsPrincipal currentUser, string username, decimal amount, BankSystemContext _context, ITransactionService _transactionService, MessageModel _messageModel)
        {

            var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            Deposits depositsExists = null;
            ChargeAccounts bankAccounts = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    depositsExists = await _context.Deposits.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id && x.Iban == deposit.Iban);
                }
                else
                {
                    _messageModel.Message = "User not found!";
                    return StatusCode(404, _messageModel);
                }

                if (depositsExists != null)
                {
                    bankAccounts = _context.ChargeAccounts.FirstOrDefault(x => x.UserId == userAuthenticate.Id);
                    return await ValidateDepositAmountAndBankAccount(userAuthenticate, depositsExists, currentUser, amount, bankAccounts, _context, _transactionService, _messageModel);
                }
                else
                {
                    _messageModel.Message = "Deposit not found";
                    return StatusCode(404, _messageModel);
                }
            }

            _messageModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, _messageModel);
        }
        public async Task<ActionResult<MessageModel>> WithdrawMoney(Deposits deposit, ClaimsPrincipal currentUser, string username, decimal amount, BankSystemContext _context, ITransactionService _transactionService, MessageModel _messageModel)
        {

            var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            Deposits depositsExists = null;

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                if (userAuthenticate != null)
                {
                    depositsExists = await _context.Deposits.FirstOrDefaultAsync(x => x.UserId == userAuthenticate.Id && x.Iban == deposit.Iban);
                }
                else
                {
                    _messageModel.Message = "User not found!";
                    return StatusCode(404, _messageModel);
                }

                if (depositsExists != null)
                {
                    depositsExists.Amount = depositsExists.Amount - amount;
                    depositsExists.PaymentDate = DateTime.Now.AddMonths(depositsExists.TermOfPayment);
                    await _context.SaveChangesAsync();

                    Transactions transaction = new Transactions();
                    transaction.SenderAccountInfo = depositsExists.Iban;
                    transaction.RecieverAccountInfo = $"{userAuthenticate.FirstName} {userAuthenticate.LastName}";
                    await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, $"Withdrawing {amount}", _context, _messageModel);
                    _messageModel.Message = "Money withdrawed successfully!";
                    return StatusCode(200, _messageModel);
                }
                else
                {
                    _messageModel.Message = "Deposit not found";
                    return StatusCode(404, _messageModel);
                }
            }

            _messageModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, _messageModel);
        }

        private async Task<ActionResult<MessageModel>> ValidateDepositAmountAndBankAccount(Users userAuthenticate, Deposits depositsExists, ClaimsPrincipal currentUser, decimal amount, ChargeAccounts bankAccounts, BankSystemContext _context, ITransactionService _transactionService, MessageModel _messageModel)
        {
            if (amount < 0)
            {
                _messageModel.Message = "Invalid payment amount!";
                return StatusCode(400, _messageModel);
            }
            else if (amount == 0)
            {
                _messageModel.Message = "Put amount more than 0.00lv";
                return StatusCode(400, _messageModel);
            }
            else
            {
                if (bankAccounts != null && bankAccounts.Amount > amount)
                {
                    depositsExists.Amount = depositsExists.Amount + amount;
                    depositsExists.PaymentDate = DateTime.Now.AddMonths(6);
                    bankAccounts.Amount = bankAccounts.Amount - amount;
                    Transactions transaction = new Transactions();
                    transaction.SenderAccountInfo = $"User {userAuthenticate.FirstName} {userAuthenticate.LastName}";
                    transaction.RecieverAccountInfo = depositsExists.Iban;
                    await _context.SaveChangesAsync();
                    await _transactionService.CreateTransaction(userAuthenticate, currentUser, amount, transaction, "Added money - Bank Account - Deposit account", _context, _messageModel);

                }
                else if (bankAccounts.Amount < amount)
                {
                    _messageModel.Message = "You don't have enough money in Bank Account!";
                    return StatusCode(406, _messageModel);
                }
                else if (bankAccounts == null)
                {
                    _messageModel.Message = "You don't have a Bank Account";
                    return StatusCode(400, _messageModel);
                }
            }
            _messageModel.Message = $"Succesfully deposited {amount} leva.";
            return StatusCode(200, _messageModel);
        }

        public async Task<ActionResult<MessageModel>> DeleteDeposit(ClaimsPrincipal currentUser, string username, BankSystemContext _context, MessageModel _messageModel)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                Deposits depositsExists = null;

                if (user != null)
                {
                    depositsExists = await _context.Deposits.FirstOrDefaultAsync(x => x.UserId == user.Id);
                }

                if (user == null)
                {
                    _messageModel.Message = "User not found!";
                    return StatusCode(404, _messageModel);
                }
                else if (depositsExists == null)
                {
                    _messageModel.Message = "User doesn't have a Deposit";
                    return StatusCode(400, _messageModel);
                }

                _context.Deposits.Remove(depositsExists);
                await _context.SaveChangesAsync();

                _messageModel.Message = $"Succsesfully deleted {user.Username} Deposit!";
                return StatusCode(200, _messageModel);
            }
            else
            {
                _messageModel.Message = "You are not authorized to do such actions";
                return StatusCode(403, _messageModel);
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
