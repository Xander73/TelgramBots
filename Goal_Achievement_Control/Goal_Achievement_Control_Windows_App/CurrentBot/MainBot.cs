using Goal_Achievement_Control_Windows_App.Helpers;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Goal_Achievement_Control.CurrentBot
{
    class MainBot : BaseBot.BaseBot
    {
        private List<Client> clients;
        private InputMessageHandler messageHandler;

        public MainBot()
        {
            clients = new List<Client>();
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
                    var updates = await Bot.GetUpdatesAsync(offset);
                    foreach (var v in updates)
                    {
                        var message = v.Message;
                        if (message == null) return;

                        //var IdCurrentClient = message.From.Id;        //ID пользователя
                        int indexCurrentClient = -1;

                        foreach (var c in clients)
                        {
                            if (c.ID == message.From.Id)
                            {
                                indexCurrentClient = clients.IndexOf(c);
                            }
                        }

                        if (indexCurrentClient == -1)       //если ID не найден, то создаем и добавляем нового клиента и присваиваем ему индек последнего объекта
                        {
                            clients.Add(new Client(message));
                            indexCurrentClient = clients.Count - 1;
                        }

                        await Bot.SendTextMessageAsync(message.Chat.Id, (clients[indexCurrentClient].Message = message).Text);
                        clients[indexCurrentClient].Message = message;  //передавем значение и в свойстве запускаем обработчик сообщений


                        //if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        //{

                        //    if (message.Text.ToLower() == "/вперед")
                        //    {
                        //        if (isStarted = clients[indexCurrentClient].Goals.Count > 3 ? true : false)
                        //        {
                        //            clients.Add(new Client(message));
                        //            await Bot.SendTextMessageAsync(message.Chat.Id, "Вы уже начали путь к достижению цели");
                        //        }
                        //        else
                        //        {
                        //            await Bot.SendTextMessageAsync(message.Chat.Id, "Введите от 3 до 15 целей");
                        //            var s = await Bot.SendTextMessageAsync(message.Chat.Id, "Enter something");
                        //            int myID = message.From.Id;
                        //            await Bot.SendTextMessageAsync(message.Chat.Id, myID.ToString());
                        //for (int i = 1; i <= 15; ++i)
                        //{
                        //    if (clients[indexCurrentClient].Goals.Count > 3)
                        //    {                                            
                        //        await Bot.SendTextMessageAsync(message.Chat.Id, $"Еще необходимо ввести {2-i} целей");
                        //    }
                        //    else
                        //    {                                            
                        //        await Bot.SendTextMessageAsync(message.Chat.Id, @"Для прекращения введите /stop");
                        //    }
                        //}


                        //while (true)
                        //{
                        //    var keyBoard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup
                        //    (
                        //    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
                        //    {
                        //        new []
                        //        {
                        //            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData ("Встряхнуть", "callBack1"),

                        //                    Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData("Положить", "callBack2"),
                        //                },
                        //            }
                        //            );
                        //    await Bot.SendTextMessageAsync(message.Chat.Id, "Инлайн кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                        //    if (clients[indexCurrentClient].Goals.Count > 2)
                        //    {
                        //        break;

                        //    }
                        //}
                        //    }
                        //}

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
                        //}
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
