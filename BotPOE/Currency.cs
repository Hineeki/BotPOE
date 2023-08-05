using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotPOE
{
    internal class Currency
    {
        public Currency(string name, double value)
        {
            CurrencyTypeName = name;
            ChaosEquivalent = value;
        }
        public string CurrencyTypeName { get; }
        public double ChaosEquivalent { get; }
    }
}
