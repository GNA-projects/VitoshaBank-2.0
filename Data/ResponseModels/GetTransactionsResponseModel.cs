using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class GetTransactionsResponseModel
    {
        public string SenderInfo { get; set; }
        public string RecieverInfo { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
    }
}
