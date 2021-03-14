using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class ChargeAccounts
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }
    }
}
