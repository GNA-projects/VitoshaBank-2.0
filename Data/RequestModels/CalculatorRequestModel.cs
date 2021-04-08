using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.RequestModels
{
    public class CalculatorRequestModel
    {
        public string Curr1 { get; set; }
        public string Curr2 { get; set; }
    }
}
