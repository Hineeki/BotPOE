using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RequestPoeNinjaData 
{
    public class Fragment : ITradable
    {
        public Fragment(string name, double cost) 
        {
            Name = name;
            ChaosEquivalent = cost;
        }
        public string Name { get; }
        public double ChaosEquivalent { get; }
    }
}
