using Telegram.Bot;

namespace BotPOE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var botClient = new TelegramBotClient("");
            using CancellationTokenSource cts = new();
            
            Buttons buttons = new Buttons();
            var chaos = new Currency("Chaos orb", 1);
            var div = new Currency("Divine Orb", 185);
            var mirShard = new Currency("Mirror Shard", 5300);
            var mirror = new Currency("Mirror of Kalandra", 105300);//надо превратить в 105.3k
            List<Currency> listCurrency = new List<Currency>()
            {
                div, mirShard, mirror
            };
            Console.WriteLine("Выберите пункт меню: 1. Курс валют. 2. Конвертер валют.");
            Console.Write("Пункт №");
            int button = Convert.ToInt32(Console.ReadLine());

            switch (button)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Курс валют");
                    PrintCurrencyRate(listCurrency);
                    break;
                case 2:
                    Console.WriteLine("Конвертер валют");
                    Console.WriteLine("Выберите пункт : 1. Chaos to Div. 2. Div to Chaos.");
                    Console.Write("Пункт №");
                    int button_button = Convert.ToInt32(Console.ReadLine());
                    switch (button_button)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Chaos to Div");
                            double chaosValue = Convert.ToDouble(Console.ReadLine());
                            chaosValue /= div.Value;
                            Console.WriteLine(chaosValue);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Div to Chaos");
                            double divValue = Convert.ToDouble(Console.ReadLine());
                            double c = divValue * div.Value;
                            Console.WriteLine(c);
                            break;
                    }
                    break;
            }
        }
        static void PrintCurrencyRate(List<Currency> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name}: {item.Value} chaos orbes.");
            }
        }
    }
}