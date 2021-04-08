using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class WalletResponseModel
    {
        public string IBAN { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardBrand { get; set; }
    }
}
