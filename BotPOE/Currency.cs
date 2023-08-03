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
            Name = name;
            Value = value;
        }
        public string Name { get; }
        public double Value { get; }// in chaos
    }
}
