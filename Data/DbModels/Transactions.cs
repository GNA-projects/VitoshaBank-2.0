using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Transactions
    {
        public int Id { get; set; }
        public string SenderAccountInfo { get; set; }
        public string RecieverAccountInfo { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
    }
}
