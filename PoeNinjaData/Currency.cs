﻿
namespace RequestPoeNinjaData
{
    public class Currency
    {
        public Currency(string name, double value)
        {
            CurrencyTypeName = name;
            ChaosEquivalent = value;
        }
        public Currency() { }
        public string CurrencyTypeName { get; }
        public double ChaosEquivalent { get; }
    }
}