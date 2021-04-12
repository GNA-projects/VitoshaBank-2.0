using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Services.GenerateCardInfoService
{
    public static class GenerateCardInfo
    {
        public static string GenerateNumber(int number)
        {
            Random random = new Random();
            const string chars = "0123456789";
            var serial = (Enumerable.Repeat(chars, number)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            Random semiRandom = new Random();
            const string nums = "45";
            var type = (Enumerable.Repeat(nums, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return $"{string.Join("", type)}{string.Join("", serial)}";
        }

        public static string GenerateCVV(int number)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, number)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
