using Goal_Achievement_Control_Windows_App.Helpers;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Goal_Achievement_Control_Windows_App.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using Telegram.Bot;

namespace Goal_Achievement_Control.CurrentBot
{
    public enum OperatingMode { AddGoal, DeleteGoal, Error, NON, AddMark };

    class MainBot : BaseBot.BaseBot
    {
        private InputMessageHandler messageHandler;
        public DataBase dataBase;
        
                    private readonly DateTime timeCheckingAssessmenGoals = new DateTime();

        public MainBot()
        {
            dataBase = new DataBase("Goal_Achievement_Control");
            Timer();
            SendToBot("1", "1", "1");
            
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

                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset);
                    foreach (var v in updates)
                    {
                        var message = v.Message;
                        if (message == null) return;
                                                
                        int idCurrentUser = dataBase.IdCurrentUser(message.From.Id);

                        if (message.Text == "/й")
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Работает");
                        }    
                        
                        //if (idCurrentUser == 0)       //если ID не найден, то создаем и добавляем нового клиента и присваиваем ему индек последнего объекта
                        //{
                        //    dataBase.AddUser(message.From.Id, message.Chat.Id, OperatingMode.AddGoal);  //add new user    
                        //    await Bot.SendTextMessageAsync(message.Chat.Id, "Приветствуем Вас.\nВведите от 3 до 15 целей, которые необходимо достичь.");
                        //}
                        //else
                        //{
                        //    User user = new User(dataBase, idCurrentUser, message);
                        //    await Bot.SendTextMessageAsync(message.Chat.Id, (user.Message = message).Text);
                        //}
                        {
                            //clients[indexCurrentClient].Message = message;  //передавем значение и в свойстве запускаем обработчик сообщений


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

        //-------------------
        private void Timer ()
        {
            CheckingAssessmenGoalsToday();            
        }


        private async void CheckingAssessmenGoalsToday()
        {
            await Task.Run(() => CheckingAssessmenGoalsTodayAsync());   //асинхронно запускаем проверку пользователей на ежедневный ввод оценок
        }

        private void CheckingAssessmenGoalsTodayAsync()
        {
            TaskCompletionSource<int> tsk = new TaskCompletionSource<int>();
            DateTime dateLastCheck = DateTime.Now.AddDays(-1);
            while (true)
            {
                bool checkToday = false;

                if (dateLastCheck != DateTime.Now.Date)
                {
                    checkToday = false;
                }

                if (DateTime.Now.Hour >= timeCheckingAssessmenGoals.Hour && !checkToday)
                {
                    using (var Connection = new SQLiteConnection ($"Data Source = {dataBase.NameDataBase}"))
                    {
                        Connection.Open();
                        using (var cmd = Connection.CreateCommand())
                        {
                            int maxUsers;
                            cmd.CommandText = "SELECT MAX(id) from Users";
                            using (var reader = cmd.ExecuteReader())
                            {
                                reader.Read();
                                maxUsers = reader[0] is int mu ? mu : (-1);   
                            }

                            if (maxUsers != (-1))
                            {
                                for (int i = 1; i <= maxUsers; ++i)
                                {
                                    cmd.CommandText = $"SELECT Goal FROM Goals WHERE userId == {i} AND isMarked == false";
                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        List<string> tempGoals = new List<string>();
                                        while (reader.Read())
                                        {
                                            
                                            if (reader["Goal"] is string s)
                                            {
                                                tempGoals.Add(s);
                                            }
                                            else
                                            {
                                               // await 
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    dateLastCheck = DateTime.Now.Date;
                    checkToday = true;
                    Thread.Sleep(8640000); //8640000 - милисекунды в сутках
                }
                else
                {
                    Thread.Sleep(timeCheckingAssessmenGoals.Millisecond - DateTime.Now.TimeOfDay.Milliseconds);
                }
            }
        }

        private async Task SendToBot (string token, string idUser, string text)
        {
            string id = "1283387864";
            string tok = "859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M";

            TelegramBotClient bot = new TelegramBotClient("859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M");
            var v = await bot.SendTextMessageAsync(id, "/1");

            //bot.DeleteMessageAsync(id, v.MessageId);
        }
    }
}
