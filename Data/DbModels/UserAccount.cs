using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class UserAccount
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserUsername { get; set; }
        public int? ChargeAccountId { get; set; }
        public int? CreditId { get; set; }
        public int? DepositId { get; set; }
        public int? WalletId { get; set; }

        public virtual ChargeAccount ChargeAccount { get; set; }
        public virtual Credit Credit { get; set; }
        public virtual Deposit Deposit { get; set; }
        public virtual User User { get; set; }
        public virtual User UserUsernameNavigation { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
