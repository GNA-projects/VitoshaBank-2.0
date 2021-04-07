using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.RequestModels
{
    public class CreditRequestModel
    {
        public string Username { get;  set; }
        public Credit Credit { get;  set; }
        public int Period { get;  set; }
        public decimal Amount { get;  set; }
        public string Product { get;  set; }
        public string Reciever { get;  set; }
        public ChargeAccount ChargeAccount { get;  set; }
    }
}
