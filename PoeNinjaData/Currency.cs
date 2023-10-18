
namespace RequestPoeNinjaData
{
    public class Currency : ITradable
    {
        public Currency(string name, double cost)
        {
            Name = name;
            ChaosEquivalent = cost;
        }
        public string Name { get; }
        public double ChaosEquivalent { get; }
    }
}
