using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLogic
{
    public class KeyBoard
    {
        private TelegramBotClient _botClient;
        private Chat _chat;
        public KeyBoard(TelegramBotClient botClient, Chat chat) 
        {
            _botClient = botClient;
            _chat = chat;
        }
        //static List<KeyboardButton> buttons1 = new List<KeyboardButton>()
        //{ new KeyboardButton("Курс валют"), new KeyboardButton("Конвертер валют") };
    }
}
