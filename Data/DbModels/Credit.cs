using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Credit
    {
        public Credit()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public decimal Interest { get; set; }
        public decimal Instalment { get; set; }
        public decimal CreditAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal CreditAmountLeft { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
