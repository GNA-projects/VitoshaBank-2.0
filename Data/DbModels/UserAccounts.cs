using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class UserAccounts
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserUsername { get; set; }
        public int? ChargeaccountId { get; set; }
        public int? CreditId { get; set; }
        public int? DepositId { get; set; }
        public int? WalletId { get; set; }
        public int? SupportId { get; set; }
    }
}
