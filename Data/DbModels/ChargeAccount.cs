using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class ChargeAccount
    {
        public ChargeAccount()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }

        public virtual Card Card { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
