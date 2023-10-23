using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace RequestPoeNinjaData
{
    public class GetPoeData
    {
        public static string? leagueName = "";
        public static List<Currency> currencies = new List<Currency>();
        public static List<Fragment> fragments = new List<Fragment>();
        public static List<DivinationCard> divinationCards = new List<DivinationCard>();
        public static Currency div = new Currency(null,0.00);

        public static string GetLeagueData()
        {
            GetRequest leagueRequest = new GetRequest("https://poe.ninja/api/data/getindexstate?");
            leagueRequest.Run();
            string leagueResponse = leagueRequest.Response;
            JObject jsonLeagues = JObject.Parse(leagueResponse);
            string oldLeagueName = leagueName;
            leagueName = GetLeagueName(jsonLeagues);
            if (string.IsNullOrEmpty(leagueName))
            {
                Console.WriteLine("Данные с WEB по лиге не получены.");
            }
            return leagueName ??= oldLeagueName; //сомнительно!!!
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
            JObject jsonCurrency = JObject.Parse(currencyRequest.Response);
            List<Currency> newList = new List<Currency>();
            foreach (var item in jsonCurrency["lines"])
            {
                newList.Add(new Currency((string)item["currencyTypeName"], (double)item["chaosEquivalent"]));
            }
            if (newList[0].Name != "Mirror of Kalandra")
            {
                Console.WriteLine("Данные с WEB по валюте не получены.");
            }
            return newList;
        }
        public static List<Fragment> GetFragmentsData(string leagueName)
        {
            GetRequest fragmentsRequest = new GetRequest($"https://poe.ninja/api/data/currencyoverview?league={leagueName}&type=Fragment");
            fragmentsRequest.Run();
            JObject jsonFragments = JObject.Parse(fragmentsRequest.Response);
            List<Fragment> newList = new List<Fragment>();
            foreach (var item in jsonFragments["lines"])
            {
                newList.Add(new Fragment((string)item["currencyTypeName"], (double)item["chaosEquivalent"]));
            }
            if (newList[0].Name != "Decaying Reliquary Key")
            {
                Console.WriteLine("Данные с WEB по валюте не получены.");
            }
            return newList;
        }
        public static List<DivinationCard> GetDivCardsData(string leagueName)
        {
            GetRequest divCardsRequest = new GetRequest(
                $"https://poe.ninja/api/data/itemoverview?league={leagueName}&type=DivinationCard");
            divCardsRequest.Run();
            JObject jsonDivCards = JObject.Parse(divCardsRequest.Response);
            List<DivinationCard> newList = new List<DivinationCard>();
            foreach (var item in jsonDivCards["lines"])
            {
                newList.Add(new DivinationCard((string)item["name"], (double)item["chaosValue"]));
            }
            if (newList[0].Name != "House of Mirrors")
            {
                Console.WriteLine("Данные с WEB по валюте не получены.");
            }
            return newList;
        }

        public static StringBuilder GetStringBuilder(IEnumerable<ITradable> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                if (item.ChaosEquivalent > 100)
                {
                    sb.AppendLine($"{item.Name}: {item.ChaosEquivalent} chaos orbes.");
                }
            }
            return sb;
        }
    }
}
