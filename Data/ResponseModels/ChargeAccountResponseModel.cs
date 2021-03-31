using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class ChargeAccountResponseModel
    {
        public string IBAN { get; set; }
        public decimal Amount { get; set; }

    }
}
