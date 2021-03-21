﻿using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Cards
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int ChargeAccountId { get; set; }
        public DateTime CardExpirationDate { get; set; }

        public virtual ChargeAccounts ChargeAccount { get; set; }
    }
}