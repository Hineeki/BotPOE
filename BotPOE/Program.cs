using Telegram.Bot;
using System.Timers;
using Telegram.Bot.Polling;
using RequestPoeNinjaData;
using BotLogic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;

namespace BotPOE
{
    internal class Program
    {
        private static System.Timers.Timer myTimer;
        static async Task Main(string[] args)
        {
            myTimer = new System.Timers.Timer(600000);
            myTimer.Elapsed += OnTimerElapsed;
            myTimer.Start();
            GetPoeData.myCurrency = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());
            GetPoeData.div = GetPoeData.myCurrency.Find(x => x.CurrencyTypeName == "Divine Orb");
            GetPoeData.StringCurrency = GetPoeData.GetStringBuilder(GetPoeData.myCurrency);
            
            Console.WriteLine("Запущен бот " + BasicLogic.bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            BasicLogic.bot.StartReceiving(
            updateHandler: BasicLogic.HandleUpdateAsync,
            pollingErrorHandler: BasicLogic.HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types //можно(скорее даже нужно) задать только необходимые типы для recуiver
            },
            cancellationToken: cts.Token);

            Console.ReadLine();
            cts.Cancel();
        }

        private static void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Отправка запросов к веб-серверу. Период 10 минут.");
            GetPoeData.myCurrency = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());
        }
    }
}