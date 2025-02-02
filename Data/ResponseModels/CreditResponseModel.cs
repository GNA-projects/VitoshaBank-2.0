﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class CreditResponseModel
    {
        public string IBAN { get; internal set; }
        public decimal Amount { get; internal set; }
        public object Instalment { get; internal set; }
        public decimal CreditAmount { get; internal set; }
    }
}
