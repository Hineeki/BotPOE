using Newtonsoft.Json.Linq;
using RequestPoeNinjaData;
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

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)//is not { } message - это значит, что если update пустой, то return,
                return;                           //а если не пустой то создаётся переменная message,ниже по томуже принципу

            // Only process text messages
            if (message.Text is not { } messageText)
                return;
            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            try
            {
                switch (messageText)
                {
                    case "Курс валют":
                        Message message1 = await botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: GetPoeData.StringCurrency.ToString(),// ситуация РВГ
                            cancellationToken: cancellationToken);
                        break;
                    case ("Chaos to Div"):
                        Message convertMessage1 = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите сумму для конвертации:",
                            cancellationToken: cancellationToken);
                        break;
                    case ("Div to Chaos"):
                        Message convertMessage2 = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите сумму для конвертации:",
                            cancellationToken: cancellationToken);
                        break;
                    default:
                        Message defaultMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "You said:\n" + messageText,
                            replyMarkup: keyboard1,
                            cancellationToken: cancellationToken);
                        break;
                }

                //Доделать!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (double.TryParse(messageText, out double amount))
                {
                    await ChaosToDiv(botClient, message);
                    await DivToChaos(botClient, message);

                    Message message1 = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "я над этим работаю. convValue =" + amount,
                            cancellationToken: cancellationToken);
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
            var convValue = Convert.ToInt32(message.Text);
            var value = convValue / GetPoeData.div.ChaosEquivalent;
            await botClient.SendTextMessageAsync(message.Chat.Id, Convert.ToString(value));
        }
        private static async Task DivToChaos(ITelegramBotClient botClient, Message message)
        {
            var convValue = Convert.ToInt32(message.Text);
            var value = convValue * GetPoeData.div.ChaosEquivalent;
            await botClient.SendTextMessageAsync(message.Chat.Id, Convert.ToString(value));
        }

    }
}