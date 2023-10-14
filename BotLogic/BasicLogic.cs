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
        static List<KeyboardButton> buttons1 = new List<KeyboardButton>() { new KeyboardButton("Курс валют"), new KeyboardButton("Конвертер валют") };
        static List<KeyboardButton> buttons2 = new List<KeyboardButton>() { new KeyboardButton("Chaos to Div"), new KeyboardButton("Div to Chaos") };
        static ReplyKeyboardMarkup keyboard1 = new ReplyKeyboardMarkup(buttons1) {ResizeKeyboard = true };
        static ReplyKeyboardMarkup keyboard2 = new ReplyKeyboardMarkup(buttons2) {ResizeKeyboard = true };

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)//is not { } message - это значит, что если update пустой, то return,
                return;                           //а если не пустой то создаётся переменная message,ниже по томуже принципу

            // Only process text messages
            if (message.Text is not { } messageText)
                return;
            string convertMode = "";
            static double _convertValue = 0.0;
            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            try
            {
                if(messageText == "Курс валют")
                {
                    Message message1 = await botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: GetPoeData.StringCurrency.ToString(),// ситуация РВГ
                            cancellationToken: cancellationToken);
                }
                else if(messageText == "Конвертер валют")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Выберите направление конвертации:",
                            replyMarkup: keyboard2,
                            cancellationToken: cancellationToken);
                }
                else if (messageText == "Chaos to Div" || messageText == "Div to Chaos")
                {
                    Message convertMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите сумму для конвертации:",
                            cancellationToken: cancellationToken);
                            convertMode = messageText;
                }
                //Доделать!!!!
                else if (double.TryParse(messageText, out double amount))
                {
                    double convValue = amount;
                    await TryConvert(convertMode, amount);

                    Message message1 =  await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: _convertValue.ToString(),
                            cancellationToken: cancellationToken);

                            convertMode = ""; // сброс режима конвертации
                }
                else 
                {
                    Message defaultMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "You said:\n" + messageText,
                            replyMarkup: keyboard1,
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

        private static async Task TryConvert(string mode,double value)
        {
            if (mode == "Chaos to Div")
            {
                 value /= GetPoeData.div.ChaosEquivalent;
            }
            else if (mode == "Div to Chaos")
            {
                value *= GetPoeData.div.ChaosEquivalent;
            }
        }
    }
}