using Goal_Achievement_Control.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Goal_Achievement_Control.MainBot
{
    class MainBot : BaseBot.BaseBot
    {
        MainBot ()
        {
            goals = new List<Goal>();
        }

        private List<Goal> goals;

        public Goal Goal
        {
            get
            {
                for (int i = 0; i < goals.Count; ++i)
                {
                    Console.WriteLine(@"{i+1} v.Name;");
                }
                Console.WriteLine("Enter the target or the index");

                string searchingTarget = Console.ReadLine().ToLower();
                if (int.TryParse(searchingTarget, out int targetindex))
                {
                    if (targetindex - 1 >= 0 && targetindex - 1 < 15)
                    {
                        return goals[targetindex];
                    }

                    else 
                    {
                        Console.WriteLine("Index is wrong");
                        return null;
                    }
                }
                else 
                {
                    foreach (var v in goals)
                    {
                        if (v.Name == searchingTarget)
                        {
                            return v;
                        }
                        else
                        {
                            Console.WriteLine("The target not found.");
                        }
                    }
                }

                Console.WriteLine("The name is not finde");
                return null;    //if not index and not finde a Name of target
            }
            set
            {
                if (goals.Count < 15)
                {
                    goals.Add(value);
                    Console.WriteLine("Goal added.");
                }
                else
                {
                    Console.WriteLine("To much goals");
                }
            }
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

                bool isStarted = false;

                while (true)
                {
                    isStarted = goals.Count > 3 ? true : false;

                    var updates = await Bot.GetUpdatesAsync(offset);
                    foreach (var v in updates)
                    {
                        var message = v.Message;
                        if (message == null) return;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                            if (message.Text.ToLower() == "/вперед")
                            {
                                if (isStarted = goals.Count > 3 ? true : false)
                                {
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Вы уже начали путь к достижению цели", replyToMessageId: message.MessageId);
                                }
                                else
                                {
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите gthdst 3 цели", replyToMessageId: message.MessageId);
                                    while (true)
                                    {
                                        if (goals.Count > 2)
                                        {
                                            break;
                                        }

                                    }
                                }
                            }
                            {
                                //if (message.Text == "/saysomething")    //answer
                                //{
                                //    // в ответ на команду /saysomething выводим сообщение
                                //    await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                                //}
                                //if (message.Text == "/ibuttons")    //inline buttons
                                //{
                                //    var keyBoard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup
                                //        (
                                //        new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
                                //        {
                                //            new []
                                //            {
                                //                Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData ("Встряхнуть", "callBack1"),

                                //                Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData("Положить", "callBack2"),
                                //            },
                                //        }
                                //        );
                                //    await Bot.SendTextMessageAsync(message.Chat.Id, "Инлайн кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                                //}
                                //if (message.Text == "/rbuttons" || message.Text == "/to_shake" || message.Text == "Встряхнуть!")
                                //{
                                //    var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Встряхнуть!"));
                                //    await Bot.SendTextMessageAsync(message.Chat.Id, processingAndReturnReply(), Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                                //}
                            }

                            //if (message.Text.ToLower() == "/changeTarget" && message.Text.ToLower() == "изменить цель")    //answer
                            //{
                            //    // в ответ на команду /saysomething выводим сообщение
                            //    await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                            //}
                        }
                        offset = v.Id + 1;
                    }

                }
            }

            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
