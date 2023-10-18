using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestPoeNinjaData
{
    public class DivinationCard : ITradable
    {
        public DivinationCard(string name, double cost) 
        {
            Name = name;
            ChaosEquivalent = cost;
        }

        public string Name { get; }
        public double ChaosEquivalent { get; }
    }
}
