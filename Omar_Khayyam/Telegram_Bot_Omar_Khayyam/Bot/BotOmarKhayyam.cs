using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using Telegram_Bot_Omar_Khayyam.Interfaces;
using static Telegram_Bot_Omar_Khayyam.Interfaces.BaseBot;

namespace Telegram_Bot_Omar_Khayyam.Bot
{
    class BotOmarKhayyam : BaseBot
    {

        private string[] Poems;

        public BotOmarKhayyam()
        {
            initilizePoems();
        }

        public override async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as string;
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key);
                await Bot.SetWebhookAsync("");
                int offset = 0;

                var ud = await Bot.GetUpdatesAsync(offset);

                //foreach (var v in ud)
                //{
                //    //var mes = v.Message;
                //    //var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Раз"));
                //    //await Bot.SendTextMessageAsync(mes.Chat.Id, "Reply кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                //}

                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset);
                    foreach (var v in updates)
                    {
                        var message = v.Message;
                        if (message == null) return;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                            if (message.Text == "/saysomething")    //answer
                            {
                                // в ответ на команду /saysomething выводим сообщение
                                await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
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
                                var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup (new[] { new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Встряхнуть!") }, true, false);
                                await Bot.SendTextMessageAsync(message.Chat.Id, processingAndReturnReply(), Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                            }
                        }

                        //callBacks

                        offset = v.Id + 1;
                    }

                }
            }

            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string processingAndReturnReply()
        {
            Random rn = new Random();   //random poem
            return Poems[rn.Next(0, 1305)];
        }

        private void initilizePoems()
        {
            Poems = new string[1306];    //1306 poems in file
            string[] poemsTemp = Properties.Resources.Poems.Split('\n');
            //WindowsFormsApp1.Properties.Resources.Poems.Split('\n');
            for (int i = 0; i < poemsTemp.Length; ++i)
            {
                //all poems are separated by a line with numbers
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
