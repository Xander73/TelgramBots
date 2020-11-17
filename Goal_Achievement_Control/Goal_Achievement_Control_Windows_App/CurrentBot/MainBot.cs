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
using System.Linq;

namespace Goal_Achievement_Control.CurrentBot
{
    public enum OperatingMode { AddGoal, DeleteGoal, Error, NON, AddMark };

    class MainBot : BaseBot.BaseBot
    {
        private InputMessageHandler messageHandler;
        public DataBase dataBase;
        
                    private readonly int timeCheckingAssessmenGoals = 19;

        public MainBot()
        {
            dataBase = new DataBase("Goal_Achievement_Control");
            TimerAsync();
            
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

        private async void TimerAsync()
        {
            await Task.Run(() => CheckingAssessmenGoalsToday());   //асинхронно запускаем проверку пользователей на ежедневный ввод оценок
        }

        private void CheckingAssessmenGoalsToday()
        {
            DataBase db = new DataBase("myDB");
            DateTime dateLastCheck = DateTime.Today.AddHours(19).AddDays(-1);     //настоящий
            while (true)
            {
                Console.WriteLine();
                if (DateTime.Now.Hour >= dateLastCheck.Hour && DateTime.Now.Date > dateLastCheck.Date)      //если текущее время больше времени проверки и сегодняшняя дата больше даты последней проверки
                {
                    {
                        using (var Connection = new SQLiteConnection($"Data Source=MyDB.db"))
                        {
                            Connection.Open();
                            using (var cmd = Connection.CreateCommand())
                            {
                                Dictionary<string, string> telegramIdUsers = new Dictionary<string, string>();
                                cmd.CommandText = "SELECT telegramId, id from Users";
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        telegramIdUsers.Add(reader["telegramId"].ToString(), reader["id"].ToString());
                                    }
                                }

                                if (telegramIdUsers.Count != (0))
                                {
                                    for (int i = 0; i < telegramIdUsers.Count; ++i)
                                    {
                                        cmd.CommandText = $"SELECT Goal FROM Goals WHERE userId == {Convert.ToInt32(telegramIdUsers.ElementAt(i).Value)} AND isMarked == false";    //userId - id пользователя в таблице Users 
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
                                                    break;
                                                }
                                            }
                                            string goals = null;
                                            foreach (var v in db.GetGoals(Convert.ToInt32(telegramIdUsers.ElementAt(i).Value)))    //DataBase.GetGoals(ID) возвращает Dictionary<int, string>, для вывода целей 
                                            {
                                                goals += v.Value + '\n';     //выведем значения и объединим их в одну строку
                                            }
                                            string messageToUser = $"Вы не поставили отметку следующим целям:\n{goals}\nДля ввода оценок введите команду - /ввести оценки";
                                            SendToBotAsync(token, telegramIdUsers.ElementAt(i).Key, messageToUser).Wait();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine("if");
                    int d = Math.Abs((int)dateLastCheck.Subtract(DateTime.Now).TotalMilliseconds);
                    if (d > 86400000)       //86400000 - милисекунды в сутках
                    {
                        d -= 86400000;
                    }
                    Console.WriteLine((24 * 60 * 60 * 1000) - d + 3600000);
                    Thread.Sleep((24 * 60 * 60 * 1000) - d + 3600000); //(24 * 60 * 60) * 1000 - милисекунды в сутках + 3600000 милисекунд - поправочный коэффициент, т.к. время начинается с нуля
                    dateLastCheck = dateLastCheck.AddDays(1);
                }
                else
                {
                    int d = Math.Abs((int)dateLastCheck.Subtract(DateTime.Now).TotalMilliseconds);
                    if (d > 86400000)       //86400000 - милисекунды в сутках
                    {
                        d -= 86400000;
                    }
                    Thread.Sleep((24 * 60 * 60 * 1000) - d + 3600000); //(24 * 60 * 60) * 1000 - милисекунды в сутках + 3600000 милисекунд - поправочный коэффициент, т.к. время начинается с нуля
                }
            }
        }
        
        private async Task SendToBotAsync (string token, string idUser, string text)
        {
            //string id = "1283387864";
            //string tok = token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    TelegramBotClient bot = new TelegramBotClient(token);
                    var v = await bot.SendTextMessageAsync(idUser, text);
                    //bot.DeleteMessageAsync(id, v.MessageId);
                }
                catch (ArgumentException e)
                {
                    System.IO.File.AppendAllText ("Connect.Error.txt", $"{e.Message}\n");
                }

                catch (Exception e)
                {
                    System.IO.File.AppendAllText("ConnectError.txt", $"{e.Message}\n");
                }
            }
        }
    }
}
