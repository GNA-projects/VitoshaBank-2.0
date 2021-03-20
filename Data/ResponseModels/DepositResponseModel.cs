using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class DepositResponseModel
    {
        public string IBAN { get; internal set; }
        public decimal Amount { get; internal set; }
        public DateTime PaymentDate { get; internal set; }
    }
}
