using Newtonsoft.Json.Linq;
using Telegram.Bot;
using System.Timers;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using RequestPoeNinjaData;
using System;

namespace BotPOE
{
    internal class Program
    {
        private static System.Timers.Timer myTimer;
        private static TelegramBotClient botClient = new TelegramBotClient("6054284848:AAEL6eVxR94H-HMZ9DqMTCddv7Fxa_u78hk");
        static async Task Main(string[] args)
        {
            myTimer = new System.Timers.Timer(600000);
            myTimer.Elapsed += OnTimerElapsed;
            myTimer.Start();
            GetPoeData.myCurrency = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());
            var div = GetPoeData.myCurrency.Find(x => x.CurrencyTypeName == "Divine Orb");

            #region Console type
            Console.WriteLine("Выберите пункт меню: 1. Курс валют. 2. Конвертер валют.");
            Console.Write("Пункт №");
            int button = Convert.ToInt32(Console.ReadLine());

            switch (button)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Курс валют");
                    PrintCurrencyRate(GetPoeData.myCurrency);
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
                            chaosValue /= div.ChaosEquivalent;
                            Console.WriteLine(chaosValue);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Div to Chaos");
                            double divValue = Convert.ToDouble(Console.ReadLine());
                            double c = divValue * div.ChaosEquivalent;
                            Console.WriteLine(c);
                            break;
                    }
                    break;
            }
            #endregion

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(//именовынные параметры
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var myBot = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{myBot.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        private static void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Отправка запросов к веб-серверу. Период 10 минут.");
            GetPoeData.myCurrency = GetPoeData.GetCurrencyData(GetPoeData.GetLeagueData());
        }

        public static void PrintCurrencyRate(List<Currency> list)
        {
            foreach (var item in list)
            {
                if(item.ChaosEquivalent > 20)
                {
                    Console.WriteLine($"{item.CurrencyTypeName}: {item.ChaosEquivalent} chaos orbes.");
                }
            }
        }

        //var chatId = 354772242;
    }
}