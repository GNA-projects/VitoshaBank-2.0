using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class DepositResponseModel
    {
        public string IBAN { get;  set; }
        public decimal Amount { get;  set; }
        public DateTime PaymentDate { get;  set; }
        public decimal Divident { get;  set; }
    }
}
