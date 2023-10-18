using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestPoeNinjaData
{
    public interface ITradable
    {
        public string Name { get; }
        public double ChaosEquivalent {  get; }
    }
}
