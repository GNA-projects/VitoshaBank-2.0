using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Services.IbanGenereatorService
{
    public static class IBANGenerator
    {
        public static string GenerateIBANInVitoshaBank(string BankAccountType, BankSystemContext dbContext)
        {
            string countryCode = "BG";
            string uniqueNumber = "18";
            string bankBIC = "VITB";
            string secondUniqueNumber = "1234567";
            string bankAccountTypeCode = "";

            if (BankAccountType == "ChargeAccount")
            {
                bankAccountTypeCode = "01";
            }
            else if (BankAccountType == "Credit")
            {
                bankAccountTypeCode = "02";
            }
            else if (BankAccountType == "Deposit")
            {
                bankAccountTypeCode = "03";
            }
            else if (BankAccountType == "Wallet")
            {
                bankAccountTypeCode = "04";
            }
            else
            {
                return null;
            }
            string currentBankAccountNumber = GetCurrentAvailabeAccountNumber(BankAccountType, dbContext);
            string IBAN = $"{countryCode}{uniqueNumber}{bankBIC}{secondUniqueNumber}{bankAccountTypeCode}{currentBankAccountNumber}";
            return IBAN;
        }
        private static string GetCurrentAvailabeAccountNumber(string BankAccountType, BankSystemContext dbContext)
        {

            if (BankAccountType == "BankAccount")
            {
                var lastBankAccount = dbContext.ChargeAccounts.ToList();
                int serialNumber = 0;
                if (lastBankAccount.Count() == 0)
                {
                    serialNumber = 1;
                }
                else
                {
                    serialNumber = lastBankAccount.LastOrDefault().Id + 1;
                }

                if (serialNumber >= 10 && serialNumber < 100)
                {
                    return $"0000{serialNumber}";
                }
                else if (serialNumber < 10)
                {
                    return $"00000{serialNumber}";
                }

                else if (serialNumber >= 100 && serialNumber < 1000)
                {
                    return $"000{serialNumber}";
                }
                else if (serialNumber >= 1000 && serialNumber < 10000)
                {
                    return $"00{serialNumber}";
                }
                else if (serialNumber >= 10000 && serialNumber < 100000)
                {
                    return $"0{serialNumber}";
                }
                else if (serialNumber >= 100000 && serialNumber < 1000000)
                {
                    return $"{serialNumber}";
                }
                else
                    return null;
            }
            else if (BankAccountType == "Credit")
            {
                var lastCredit = dbContext.Credits.ToList();
                int serialNumber = 0;
                if (lastCredit.Count() == 0)
                {
                    serialNumber = 1;
                }
                else
                {
                    serialNumber = lastCredit.LastOrDefault().Id + 1;
                }

                if (serialNumber >= 10 && serialNumber < 100)
                {
                    return $"0000{serialNumber}";
                }
                else if (serialNumber < 10)
                {
                    return $"00000{serialNumber}";
                }

                else if (serialNumber >= 100 && serialNumber < 1000)
                {
                    return $"000{serialNumber}";
                }
                else if (serialNumber >= 1000 && serialNumber < 10000)
                {
                    return $"00{serialNumber}";
                }
                else if (serialNumber >= 10000 && serialNumber < 100000)
                {
                    return $"0{serialNumber}";
                }
                else if (serialNumber >= 100000 && serialNumber < 1000000)
                {
                    return $"{serialNumber}";
                }
                else
                    return null;
            }
            else if (BankAccountType == "Deposit")
            {
                var lastDeposit = dbContext.Deposits.ToList();
                int serialNumber = 0;
                if (lastDeposit.Count() == 0)
                {
                    serialNumber = 1;
                }
                else
                {
                    serialNumber = lastDeposit.LastOrDefault().Id + 1;
                }

                if (serialNumber >= 10 && serialNumber < 100)
                {
                    return $"0000{serialNumber}";
                }
                else if (serialNumber < 10)
                {
                    return $"00000{serialNumber}";
                }

                else if (serialNumber >= 100 && serialNumber < 1000)
                {
                    return $"000{serialNumber}";
                }
                else if (serialNumber >= 1000 && serialNumber < 10000)
                {
                    return $"00{serialNumber}";
                }
                else if (serialNumber >= 10000 && serialNumber < 100000)
                {
                    return $"0{serialNumber}";
                }
                else if (serialNumber >= 100000 && serialNumber < 1000000)
                {
                    return $"{serialNumber}";
                }
                else
                    return null;
            }
            else if (BankAccountType == "Wallet")
            {
                var lastWallet = dbContext.Wallets.ToList();
                int serialNumber = 0;
                if (lastWallet.Count() == 0)
                {
                    serialNumber = 1;
                }
                else
                {
                    serialNumber = lastWallet.LastOrDefault().Id + 1;
                }

                if (serialNumber >= 10 && serialNumber < 100)
                {
                    return $"0000{serialNumber}";
                }
                else if (serialNumber < 10)
                {
                    return $"00000{serialNumber}";
                }

                else if (serialNumber >= 100 && serialNumber < 1000)
                {
                    return $"000{serialNumber}";
                }
                else if (serialNumber >= 1000 && serialNumber < 10000)
                {
                    return $"00{serialNumber}";
                }
                else if (serialNumber >= 10000 && serialNumber < 100000)
                {
                    return $"0{serialNumber}";
                }
                else if (serialNumber >= 100000 && serialNumber < 1000000)
                {
                    return $"{serialNumber}";
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
    }

}
