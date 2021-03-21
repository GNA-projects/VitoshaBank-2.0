using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class ChargeAccounts
    {
        public ChargeAccounts()
        {
            UserAccounts = new HashSet<UserAccounts>();
        }

        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }

        public virtual Cards Card { get; set; }
        public virtual ICollection<UserAccounts> UserAccounts { get; set; }
    }
}
