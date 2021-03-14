using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Deposits
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public decimal Divident { get; set; }
        public DateTime PaymentDate { get; set; }
        public int TermOfPayment { get; set; }
    }
}
