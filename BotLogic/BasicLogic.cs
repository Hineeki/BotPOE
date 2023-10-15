using Newtonsoft.Json.Linq;
using RequestPoeNinjaData;
using System.Globalization;
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
            Message? replyMessage = message.ReplyToMessage;
            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            try
            {
                switch (messageText) // надо на if переходить скорее всего
                {
                    case ("Курс валют"):
                        Message message1 = await botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: GetPoeData.StringCurrency.ToString(),// ситуация РВГ
                            cancellationToken: cancellationToken);
                        break;
                    case ("Chaos to Div"):
                        Message convertMessage1 = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Chaos to Div:",
                            replyMarkup: ForceReplyMarkup,
                            cancellationToken: cancellationToken);
                        break;
                    case ("Div to Chaos"):
                        Message convertMessage2 = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Div to Chaos:",
                            replyMarkup: ForceReplyMarkup,
                            cancellationToken: cancellationToken);
                        break;
                    default:
                        if (replyMessage == null)
                        {
                            await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Пустое сообщение",
                            replyMarkup: keyboard1,
                            cancellationToken: cancellationToken);
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

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
            if (double.TryParse(newMessage, out var convValue))
            {
                var value = convValue / GetPoeData.div.ChaosEquivalent;
                await botClient.SendTextMessageAsync(message.Chat.Id, Convert.ToString(value), replyMarkup: keyboard1);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, 
                    "Что-то не то ввели. (Попробуйте \"точку\" вместо \"запятой\" или наоборот)", replyMarkup: keyboard1);
            }
        }

        private static async Task DivToChaos(ITelegramBotClient botClient, Message message)
        {
            var newMessage = message.Text.Replace('.', ',');
            if (double.TryParse(newMessage, out var convValue))
            {
                var value = convValue * GetPoeData.div.ChaosEquivalent;
                await botClient.SendTextMessageAsync(message.Chat.Id, Convert.ToString(value), replyMarkup: keyboard1);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, 
                    "Что-то не то ввели. (Попробуйте \"точку\" вместо \"запятой\" или наоборот)", replyMarkup: keyboard1);
            }
        }

    }
}