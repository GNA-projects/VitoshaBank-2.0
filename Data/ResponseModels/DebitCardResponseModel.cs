using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class DebitCardResponseModel
    {
        public string CardNumber { get; internal set; }
        public string CardBrand { get; internal set; }
    }
}
