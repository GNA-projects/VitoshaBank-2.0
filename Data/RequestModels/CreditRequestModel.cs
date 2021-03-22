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
        public Credits Credit { get;  set; }
        public int Period { get;  set; }
    }
}
