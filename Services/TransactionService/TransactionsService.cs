using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.TransactionService.Interfaces;

namespace VitoshaBank.Services.TransactionService
{
    public class TransactionsService : ControllerBase, ITransactionsService
    {
        MessageModel _messageModel = new MessageModel();
        private readonly BankSystemContext _context;
        public TransactionsService(BankSystemContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> CreateTransaction(User user, ClaimsPrincipal currentUser, decimal amount, Transaction transaction, string reason)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                List<Transaction> transactions = new List<Transaction>();

                TransactionResponseModel sender = new TransactionResponseModel();
                TransactionResponseModel reciever = new TransactionResponseModel();
                if (transaction.SenderAccountInfo.Contains("BG18VITB") && transaction.SenderAccountInfo.Length == 23)
                {
                    sender.IsIBAN = true;
                    sender.SenderInfo = transaction.SenderAccountInfo;
                    reciever.RecieverInfo = transaction.RecieverAccountInfo;

                    if (transaction.RecieverAccountInfo.Contains("BG18VITB") && transaction.RecieverAccountInfo.Length == 23)
                    {
                        reciever.IsIBAN = true;
                        reciever.RecieverInfo = transaction.RecieverAccountInfo;
                    }
                }
                else if (transaction.RecieverAccountInfo.Contains("BG18VITB") && transaction.RecieverAccountInfo.Length == 23)
                {
                    reciever.IsIBAN = true;
                    sender.SenderInfo = transaction.SenderAccountInfo;
                    reciever.RecieverInfo = transaction.RecieverAccountInfo;
                }
                else
                {
                    _messageModel.Message = "Invalid arguments!";
                    return StatusCode(400, _messageModel);
                }
                //bad request
                if (sender.IsIBAN && reciever.IsIBAN)
                {
                    transaction.Reason = reason;
                    transaction.Date = DateTime.Now;
                    transaction.TransactionAmount = amount;
                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    transactions = _context.Transactions.ToList();
                    user.LastTransactionId = transactions.Last().Id;
                    await _context.SaveChangesAsync();

                    _messageModel.Message = "Money send successfully!";
                    return StatusCode(200, _messageModel);
                }
                else if (sender.IsIBAN && !reciever.IsIBAN)
                {
                    transaction.Reason = reason;
                    transaction.Date = DateTime.Now;
                    transaction.TransactionAmount = amount;
                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    transactions = _context.Transactions.ToList();
                    user.LastTransactionId = transactions.Last().Id;
                    await _context.SaveChangesAsync();

                    _messageModel.Message = "Purchase successfull!";
                    return StatusCode(200, _messageModel);
                }
                else if (!sender.IsIBAN && reciever.IsIBAN)
                {
                    transaction.Reason = reason;
                    transaction.Date = DateTime.Now;
                    transaction.TransactionAmount = amount;
                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    transactions = _context.Transactions.ToList();
                    user.LastTransactionId = transactions.Last().Id;
                    await _context.SaveChangesAsync();

                    _messageModel.Message = "Money recieved successfully!";
                    return StatusCode(200, _messageModel);
                }
            }

            _messageModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, _messageModel);

        }

        public async Task<ActionResult<GetTransactionsResponseModel>> GetTransactionInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                List<GetTransactionsResponseModel> allTransactions = new List<GetTransactionsResponseModel>();
                List<Transaction> userTransactions = new List<Transaction>();
                Dictionary<string, List<string>> userDicTransactions = new Dictionary<string, List<string>>();

                List<ChargeAccount> userChargeAccounts = _context.ChargeAccounts.Where(x => x.UserId == userAuthenticate.Id).ToList();
                List<Deposit> userDeposits = _context.Deposits.Where(x => x.UserId == userAuthenticate.Id).ToList();
                List<Credit> userCredits = _context.Credits.Where(x => x.UserId == userAuthenticate.Id).ToList();
                List<Wallet> userWallets = _context.Wallets.Where(x => x.UserId == userAuthenticate.Id).ToList();

                if (userAuthenticate == null)
                {
                    _messageModel.Message = "User not found";
                    return StatusCode(404, _messageModel);
                }
                else
                {
                    if (userChargeAccounts.Count > 0)
                    {
                        List<string> IBANs = new List<string>();
                        foreach (var chargeAccount in userChargeAccounts)
                        {
                            IBANs.Add(chargeAccount.Iban);
                        }

                        userDicTransactions.Add("ChargeAccount", IBANs);
                    }

                    if (userDeposits.Count > 0)
                    {
                        List<string> IBANs = new List<string>();
                        foreach (var deposit in userDeposits)
                        {
                            IBANs.Add(deposit.Iban);
                        }

                        userDicTransactions.Add("Deposit", IBANs);
                    }

                    if (userCredits.Count > 0)
                    {
                        List<string> IBANs = new List<string>();

                        foreach (var credit in userCredits)
                        {
                            IBANs.Add(credit.Iban);
                        }

                        userDicTransactions.Add("Credit", IBANs);
                    }

                    if (userWallets.Count > 0)
                    {
                        List<string> IBANs = new List<string>();

                        foreach (var wallet in userWallets)
                        {
                            IBANs.Add(wallet.Iban);
                        }
                        userDicTransactions.Add("Wallet", IBANs);
                    }

                    foreach (var IBAN in userDicTransactions)
                    {
                        if (IBAN.Key == "ChargeAccount" && IBAN.Value != null)
                        {
                            foreach (var IbanValue in IBAN.Value)
                            {
                                var userSender = await _context.Transactions.Where(x => x.SenderAccountInfo == IbanValue).ToListAsync();
                                var userReciver = await _context.Transactions.Where(x => x.RecieverAccountInfo == IbanValue).ToListAsync();

                                if (userSender != null)
                                {
                                    foreach (var transaction in userSender)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }

                                if (userReciver != null)
                                {
                                    foreach (var transaction in userReciver)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }
                            }
                        }
                        else if (IBAN.Key == "Deposit" && IBAN.Value != null)
                        {
                            foreach (var IbanValue in IBAN.Value)
                            {
                                var userSender = await _context.Transactions.Where(x => x.SenderAccountInfo == IbanValue).ToListAsync();
                                var userReciver = await _context.Transactions.Where(x => x.RecieverAccountInfo == IbanValue).ToListAsync();

                                if (userSender != null)
                                {
                                    foreach (var transaction in userSender)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }

                                if (userReciver != null)
                                {
                                    foreach (var transaction in userReciver)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }
                            }
                        }
                        else if (IBAN.Key == "Credit" && IBAN.Value != null)
                        {
                            foreach (var IbanValue in IBAN.Value)
                            {
                                var userSender = await _context.Transactions.Where(x => x.SenderAccountInfo == IbanValue).ToListAsync();
                                var userReciver = await _context.Transactions.Where(x => x.RecieverAccountInfo == IbanValue).ToListAsync();

                                if (userSender != null)
                                {
                                    foreach (var transaction in userSender)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }

                                if (userReciver != null)
                                {
                                    foreach (var transaction in userReciver)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }
                            }
                        }
                        else if (IBAN.Key == "Wallet" && IBAN.Value != null)
                        {
                            foreach (var IbanValue in IBAN.Value)
                            {
                                var userSender = await _context.Transactions.Where(x => x.SenderAccountInfo == IbanValue).ToListAsync();
                                var userReciver = await _context.Transactions.Where(x => x.RecieverAccountInfo == IbanValue).ToListAsync();

                                if (userSender != null)
                                {
                                    foreach (var transaction in userSender)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }

                                if (userReciver != null)
                                {
                                    foreach (var transaction in userReciver)
                                    {
                                        userTransactions.Add(transaction);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var transaction in userTransactions)
                    {
                        GetTransactionsResponseModel responseModel = new GetTransactionsResponseModel();
                        responseModel.SenderInfo = transaction.SenderAccountInfo;
                        responseModel.RecieverInfo = transaction.RecieverAccountInfo;
                        responseModel.Amount = transaction.TransactionAmount;
                        responseModel.Reason = transaction.Reason;
                        responseModel.Date = transaction.Date;

                        allTransactions.Add(responseModel);
                        allTransactions = allTransactions.OrderBy(x => x.Date).ToList();
                    }

                }

                if (allTransactions.Count == 0)
                {
                    _messageModel.Message = "User does not have transactions";
                    return StatusCode(404, _messageModel);
                }
                return StatusCode(200, allTransactions);
            }

            _messageModel.Message = "You are not autorized to do such actions!";
            return StatusCode(403, _messageModel);
        }
    }
}
