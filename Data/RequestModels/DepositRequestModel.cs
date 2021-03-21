﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.RequestModels
{
    public class DepositRequestModel
    {
        public Deposits Deposit { get; set; }
        public ChargeAccounts ChargeAccount { get;  set; }
        public decimal Amount { get;  set; }
        public string Username { get;set; }
    }
}
