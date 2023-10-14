using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace RequestPoeNinjaData
{
    public class GetPoeData
    {
        private static string? _leagueName;
        public static List<Currency> myCurrency = new List<Currency>();
        public static Currency div = new Currency();
        public static StringBuilder StringCurrency = new StringBuilder();

        public static string GetLeagueData()
        {
            GetRequest leagueRequest = new GetRequest("https://poe.ninja/api/data/getindexstate?");
            leagueRequest.Run();
            var leagueResponse = leagueRequest.Response;
            JObject jsonLeagues = JObject.Parse(leagueResponse);
            string oldLeagueName = _leagueName;
            _leagueName = GetLeagueName(jsonLeagues);
            if (string.IsNullOrEmpty(_leagueName))
            {
                Console.WriteLine("Данные с WEB по лиге не получены.");
            }
            return _leagueName ??= oldLeagueName; //сомнительно!!!
        }

        private static string GetLeagueName(JObject json)
        {
            var economyLeagues = json["economyLeagues"];
            if (economyLeagues != null)
            {
                foreach (var item in economyLeagues)
                {
                    var leagueName = item["name"];
                    return (string)leagueName;
                }
            }
            return null;
        }

        public static List<Currency> GetCurrencyData(string leagueName)
        {
            GetRequest currencyRequest = new GetRequest($"https://poe.ninja/api/data/currencyoverview?league={leagueName}&type=Currency");
            currencyRequest.Run();
            var currencyResponse = currencyRequest.Response;
            JObject jsonCurrency = JObject.Parse(currencyResponse);
            myCurrency = GetCurrencyList(jsonCurrency);
            if (myCurrency[0].CurrencyTypeName != "Mirror of Kalandra")
            {
                Console.WriteLine("Данные с WEB по валюте не получены.");
            }
            return myCurrency;
        }

        private static List<Currency> GetCurrencyList(JObject json)
        {
            List<Currency> currencies = new List<Currency>();
            foreach (var item in json["lines"])
            {
                currencies.Add(new Currency((string)item["currencyTypeName"], (double)item["chaosEquivalent"]));
            }
            return currencies;
        }//как будто ненужная херня

        public static StringBuilder GetStringBuilder(List<Currency> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                if (item.ChaosEquivalent > 100)
                {
                    sb.AppendLine($"{item.CurrencyTypeName}: {item.ChaosEquivalent} chaos orbes.");
                }
            }
            return sb;
        }
    }
}
