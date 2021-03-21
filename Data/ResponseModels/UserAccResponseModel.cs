using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.ResponseModels
{
    public class UserAccResponseModel
    {
        public ICollection<ChargeAccounts> UserChargeAcc { get; set; }
        public ICollection<DepositResponseModel> UserDeposits { get; set; }
        public ICollection<Credits> UserCredits { get; set; }
        public ICollection<Wallets> UserWallets { get; set; }

    }
}
