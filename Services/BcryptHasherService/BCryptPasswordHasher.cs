using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Services.BcryptHasherService
{
    public class BCryptPasswordHasherService
    {
        public BCryptPasswordHasherService(IOptions<PasswordHasherOptions> optionsAccessor = null)
        {

        }
        public string HashPassword(string password)
        {
            string advancedHashedPassword = password + "NeverGonnaLetYouDown";
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            return BCrypt.Net.BCrypt.HashPassword(advancedHashedPassword, salt);
        }

        public bool AuthenticateUser(Users user, Users userDB)
        {
            // check user found and verify password
            if (!BCrypt.Net.BCrypt.Verify(user.Password + "NeverGonnaLetYouDown", userDB.Password))
            {
                // authentication failed
                return false;
            }
            else
            {
                // authentication successful
                return true;
            }
        }

        public bool AuthenticateWalletCVV(Wallets wallets, Wallets walletsDB)
        {
            // check user found and verify password
            if (!BCrypt.Net.BCrypt.Verify(wallets.Cvv + "NeverGonnaLetYouDown", walletsDB.Cvv))
            {
                // authentication failed
                return false;
            }
            else
            {
                // authentication successful
                return true;
            }
        }

        public bool AuthenticateDebitCardCVV(string CVV, Cards cardsDB)
        {
            // check user found and verify password
            if (!BCrypt.Net.BCrypt.Verify(CVV + "NeverGonnaLetYouDown", cardsDB.Cvv))
            {
                // authentication failed
                return false;
            }
            else
            {
                // authentication successful
                return true;
            }
        }
    }
}
