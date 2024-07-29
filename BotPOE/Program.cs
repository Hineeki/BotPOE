using Telegram.Bot;
using System.Timers;
using Telegram.Bot.Polling;
using RequestPoeNinjaData;
using BotLogic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Net;
using static System.Net.WebRequestMethods;

namespace BotPOE
{
    internal class Program
    {
        private static System.Timers.Timer? myTimer;
        static async Task Main(string[] args)
        {
            myTimer = new System.Timers.Timer(600000);
            myTimer.Elapsed += OnTimerElapsed;
            myTimer.Start();

            GetPoeData.currencies = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());//может в мейне создавать статические листы с данными
            GetPoeData.div = GetPoeData.currencies.Find(x => x.Name == "Divine Orb");       //а не в библиотеках???
            //GetPoeData.fragments = GetPoeData.GetFragmentsData(GetPoeData.leagueName);
            //GetPoeData.divinationCards = GetPoeData.GetDivCardsData(GetPoeData.leagueName);


            Console.WriteLine("Запущен бот " + BasicLogic.bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();

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
            GetPoeData.currencies = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());
            GetPoeData.fragments = GetPoeData.GetFragmentsData(GetPoeData.leagueName);
            GetPoeData.divinationCards = GetPoeData.GetDivCardsData(GetPoeData.leagueName);
        }
    }
}