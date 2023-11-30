using Newtonsoft.Json.Linq;
using RequestPoeNinjaData;
using System.Globalization;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLogic
{
    public class BasicLogic
    {
        public static ITelegramBotClient bot = new TelegramBotClient("6054284848:AAEL6eVxR94H-HMZ9DqMTCddv7Fxa_u78hk");

        static List<KeyboardButton> buttons1 = new List<KeyboardButton>() { new KeyboardButton("Курс валют"),
                                               new KeyboardButton("Chaos to Div"), new KeyboardButton("Div to Chaos") };

        static ReplyKeyboardMarkup keyboard1 = new ReplyKeyboardMarkup(buttons1) { ResizeKeyboard = true };

        static ForceReplyMarkup ForceReplyMarkup = new ForceReplyMarkup();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)//is not { } message - это значит, что если update пустой, то return,
                return;                           //а если не пустой то создаётся переменная message,ниже по томуже принципу

            if (message.Text is not { } messageText)
                return;
            var replyMessage = message.ReplyToMessage;
            var chatId = message.Chat.Id;


            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            try
            {
                switch (messageText)
                {
                    case ("/description"):
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                            text: "Пока что этот бот может вывести \"Курс валют\" и конвертировать хаосы в диваны или диваны в хаосы.",
                            cancellationToken: cancellationToken);
                        break;
                    case ("/basecurrency"):
                        await PrintBaseCurrency(botClient,message);
                        break;
                    case ("/fragments"):
                        await PrintFragments(botClient, message);
                        break;
                    case ("/divcards"):
                        await PrintDivCards(botClient, message);
                        break;
                    case ("Курс валют"):
                        await PrintBaseCurrency(botClient, message);
                        break;
                    case ("Chaos to Div"):
                            await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Chaos to Div:",
                            replyMarkup: ForceReplyMarkup,
                            cancellationToken: cancellationToken);
                        break;
                    case ("Div to Chaos"):
                            await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Div to Chaos:",
                            replyMarkup: ForceReplyMarkup,
                            cancellationToken: cancellationToken);
                        break;
                    default:
                        if (replyMessage == null)
                        {
                            await PrintVoidMessage(botClient, message);
                        }
                        else
                        {
                            switch (replyMessage.Text)
                            {
                                case ("Chaos to Div:"):
                                    await ChaosToDiv(botClient, message);
                                    break;
                                case ("Div to Chaos:"):
                                    await DivToChaos(botClient, message);
                                    break;
                                default:
                                    await PrintVoidMessage(botClient, message);
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }
        }

        public static Task HandlePollingErrorAsync
            (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

        private static async Task ChaosToDiv(ITelegramBotClient botClient, Message message)
        {
            var newMessage = message.Text.Replace('.', ',');
            if (double.TryParse(newMessage, CultureInfo.InvariantCulture, out var convValue))
            {
                var value = convValue / GetPoeData.div.ChaosEquivalent;
                await botClient.SendTextMessageAsync(message.Chat.Id, value.ToString("#.##") + " div", replyMarkup: keyboard1);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка валидации. Введены некорректные значения.\n" +
                    " Просьба указывать запятую вместо точки(или наоборот) и не вводить букв.", replyMarkup: keyboard1);

            }
        }

        private static async Task DivToChaos(ITelegramBotClient botClient, Message message)
        {
            var newMessage = message.Text.Replace('.', ',');
            if (double.TryParse(newMessage, out var convValue))
            {
                var value = convValue * GetPoeData.div.ChaosEquivalent;
                await botClient.SendTextMessageAsync(message.Chat.Id, value.ToString("#.##") + " chaos orbes", replyMarkup: keyboard1);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка валидации. Введены некорректные значения.\n" +
                    " Просьба указывать запятую вместо точки(или наоборот) и не вводить букв.", replyMarkup: keyboard1);
            }
        }

        private static async Task PrintBaseCurrency(ITelegramBotClient botClient, Message message)
        {
            StringBuilder sb = new StringBuilder();
            sb = GetPoeData.GetStringBuilder(GetPoeData.currencies);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: sb.ToString());
        }
        private static async Task PrintFragments(ITelegramBotClient botClient, Message message)
        {
            StringBuilder sb = new StringBuilder();
            sb = GetPoeData.GetStringBuilder(GetPoeData.fragments);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: sb.ToString());
        }
        private static async Task PrintDivCards(ITelegramBotClient botClient, Message message)
        {
            StringBuilder sb = new StringBuilder();
            sb = GetPoeData.GetStringBuilder(GetPoeData.divinationCards);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: sb.ToString());
        }

        private static async Task PrintVoidMessage(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: "Пустое сообщение",
                                    replyMarkup: keyboard1);
        }
    }
}