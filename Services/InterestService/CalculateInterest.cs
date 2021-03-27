using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Services.InterestService
{
    public static class CalculateInterest
    {
        public static decimal CalculateCreditAmount(decimal amount, int period, decimal interest)
        {
            double doubleAmount = (double)(amount);
            double coef = 1 + (double)(interest) / 100;
            double creditAmount = doubleAmount * Math.Pow(coef, period);
            return (decimal)(creditAmount);
        }
        public static decimal CalculateInstalment(decimal CreditAmount, decimal interest, int period)
        {
            double coef = 1 + (double)(interest) / 100;
            double a = Math.Pow(coef, period) * (coef - 1);
            double b = Math.Pow(coef, period) - 1;
            double instalment = (double)(CreditAmount) * (a / b);
            return (decimal)(instalment);
        }
    }
}
