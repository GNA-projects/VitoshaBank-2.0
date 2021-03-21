using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Services.Dividents
{
    public static class CalculateDividentService
    {
        public static decimal GetDividentPercent(decimal Amount, int termOfPayment)
        {
            decimal dividendProcent = 0;
            if (termOfPayment == 1)
            {
                if (Amount <= 50000)
                {
                    return dividendProcent = 2.30M;
                }
                else if (Amount > 50000)
                {
                    return dividendProcent = 3M;
                }
                else
                {
                    return 0;
                }
            }
            else if (termOfPayment == 3)
            {
                if (Amount <= 50000)
                {
                    return dividendProcent = 4M;
                }
                else if (Amount > 50000)
                {
                    return dividendProcent = 4.25M;
                }
                else
                {
                    return 0;
                }
            }
            else if (termOfPayment == 6)
            {
                if (Amount <= 50000)
                {
                    return dividendProcent = 4.30M;
                }
                else if (Amount > 50000)
                {
                    return dividendProcent = 5M;
                }
                else
                {
                    return 0;
                }
            }
            else if (termOfPayment == 12)
            {
                if (Amount <= 50000)
                {
                    return dividendProcent = 4.75M;
                }
                else if (Amount > 50000)
                {
                    return dividendProcent = 5.20M;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                //invalid term

                return dividendProcent;
            }
        }

        public static decimal GetDividentAmount(decimal amount, decimal divident, int period)
        {
            double doubleAmount = (double)(amount);
            double coef = 1 + (double)(divident) / 100;
            double depositAmount = doubleAmount * Math.Pow(coef, period);
            return (decimal)(depositAmount) - amount;
        }
    }
}
