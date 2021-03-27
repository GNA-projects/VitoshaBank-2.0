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
        public decimal Amount { get; internal set; }
        public string Product { get; internal set; }
        public string Reciever { get; internal set; }
    }
}
