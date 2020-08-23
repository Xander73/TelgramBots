using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Core;

namespace WindowsFormsApp1.Bot
{
    class Magik_Omar_Khayyam_Bot : IBot
    {
        private IToken token;
        public Magik_Omar_Khayyam_Bot()
        {
            token = new Token("859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M");   //initialized only once
        }

        public void botsAnswer(Telegram.Bot.Types.Message message)
        {
            if (message.Text == "/saysomething")    //answer
            {
                // в ответ на команду /saysomething выводим сообщение
                //await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                await Bot.
            }
            if (message.Text == "/ibuttons")    //inline buttons
            {
                var keyBoard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup
                    (
                    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
                    {
                                        new []
                                        {
                                            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData ("Встряхнуть", "callBack1"),

                                            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData("Положить", "callBack2"),
                                        },
                    }
                    );
                await Bot.SendTextMessageAsync(message.Chat.Id, "Инлайн кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
            }
            if (message.Text == "/rbuttons" || message.Text == "/to_shake" || message.Text == "Встряхнуть!")
            {
                var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Встряхнуть!"));
                await Bot.SendTextMessageAsync(message.Chat.Id, processingAndReturnReply(), Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
            }
        }

        async private void botsAnswerAsync (string message)
        {

        }

        public void botsWork()
        {

        }

        private void initilizePoems()
        {
            Poems = new string[1306];    //1306 poems in file
            string[] poemsTemp = WindowsFormsApp1.Properties.Resources.Poems.Split('\n');
            for (int i = 0; i < poemsTemp.Length; ++i)
            {
                if (int.TryParse(poemsTemp[i], out int tempIndex))  //if digit, the next line for us
                {
                    while (i < poemsTemp.Length - 1 && !int.TryParse(poemsTemp[i + 1], out int o))
                    {
                        Poems[tempIndex - 1] += poemsTemp[++i] + '\n';  //add the lines of one poem
                    }
                }
            }
        }
    }
}
