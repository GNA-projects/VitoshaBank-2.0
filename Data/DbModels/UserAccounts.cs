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
        public int? ChargeAccountId { get; set; }
        public int? CreditId { get; set; }
        public int? DepositId { get; set; }
        public int? WalletId { get; set; }
        public int? SupportId { get; set; }

        public virtual ChargeAccounts ChargeAccount { get; set; }
        public virtual Credits Credit { get; set; }
        public virtual Deposits Deposit { get; set; }
        public virtual SupportTickets Support { get; set; }
        public virtual Users User { get; set; }
        public virtual Users UserUsernameNavigation { get; set; }
        public virtual Wallets Wallet { get; set; }
    }
}
