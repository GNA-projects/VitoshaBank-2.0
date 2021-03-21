﻿using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Wallets
    {
        public Wallets()
        {
            UserAccounts = new HashSet<UserAccounts>();
        }

        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public DateTime CardExpirationDate { get; set; }

        public virtual ICollection<UserAccounts> UserAccounts { get; set; }
    }
}
