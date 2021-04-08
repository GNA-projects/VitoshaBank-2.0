using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.ResponseModels
{
    public class UserAccResponseModel
    {
        public UserAccResponseModel()
        {
            UserChargeAcc = new List<ChargeAccountResponseModel>();
            UserDeposits = new List<DepositResponseModel>();
            UserCredits = new List<CreditResponseModel>();
            UserWallets = new List<WalletResponseModel>();
        }
        public ICollection<ChargeAccountResponseModel> UserChargeAcc { get; set; }
        public ICollection<DepositResponseModel> UserDeposits { get; set; }
        public ICollection<CreditResponseModel> UserCredits { get; set; }
        public ICollection<WalletResponseModel> UserWallets { get; set; }

    }
}
