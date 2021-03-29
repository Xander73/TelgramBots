using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using Telegram.Bot;
using System.Linq;
using Goal_Achievement_Control_Windows_App.Interfaces;
using Goal_Achievement_Control_Windows_App.Core;

namespace Goal_Achievement_Control_Windows_App.CurrentBot
{
    public enum OperatingMode { AddGoal, DeleteGoal, Error, NON, AddMark };

    class MainBot : BaseBot
    {
        public IDataBase dataBase;
        private const string DATA_BASE_NAME = "Goal_Achievement_Control";

        public MainBot()
        {
            dataBase = new DataBase(DATA_BASE_NAME);
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
                        
                        if (idCurrentUser == 0)       //если ID не найден, то создаем и добавляем нового клиента и присваиваем ему индек последнего объекта
                        {                            
                            dataBase.AddUser(message.From.Id.ToString(), message.Chat.Id.ToString(), OperatingMode.AddGoal);  //add new user    
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Приветствуем Вас.\nВведите от 3 до 15 целей, которые необходимо достичь.", replyMarkup: KeyBoard());
                        }
                        else
                        {
                            IUser user = new User(dataBase, idCurrentUser, message, dataBase.GetUserMod(idCurrentUser));                            
                            await Bot.SendTextMessageAsync(message.Chat.Id, (user.Message = message).Text, replyMarkup: KeyBoard());
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

        private Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup KeyBoard ()
        {
            return new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new[]
                            {
                                new []
                                {
                                    new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Добавить цель"), new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Остановить ввод целей")
                                },
                                new []
                                {
                                    new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Удалить цель"), new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Список целей")
                                }, 
                                new []
                                {
                                    new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Статистика за 4 недели"), new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Вся статистика")
                                },
                                new []
                                {
                                    new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Ввести оценки")
                                }
                            }, false, true);
        }

        private async void TimerAsync()
        {
            await Task.Run(() => CheckingAssessmenGoalsToday());   //асинхронно запускаем проверку пользователей на ежедневный ввод оценок
        }

        private async void CheckingAssessmenGoalsToday()
        {
            IDataBase db = new DataBase(DATA_BASE_NAME);
            DateTime dateLastCheck = DateTime.Today.AddHours(19).AddDays(-1);     //настоящий
            while (true)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    Dictionary <string, string> usersIdApp = new Dictionary<string, string>(dataBase.GetTelegramIdUsers());
                    foreach (var v in usersIdApp)
                    {
                        await SendToBotAsync(token, v.Key, dataBase.MarksLastFourWeeks(Convert.ToInt32(v.Value)));
                    }
                }
                if (DateTime.Now.Hour >= dateLastCheck.Hour && DateTime.Now.Date > dateLastCheck.Date)      //если текущее время больше времени проверки и сегодняшняя дата больше даты последней проверки
                {
                    
                        using (var Connection = new SQLiteConnection($"Data Source = {dataBase.NameDataBase}"))
                        {
                            Connection.Open();
                            using (var cmd = Connection.CreateCommand())
                            {
                                Dictionary<string, string> telegramIdUsers = dataBase.GetTelegramIdUsers();                                

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
                    int d = Math.Abs((int)dateLastCheck.Subtract(DateTime.Now).TotalMilliseconds);
                    if (d > 86400000)       //86400000 - милисекунды в сутках
                    {
                        d -= 86400000;
                    }
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
            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    TelegramBotClient bot = new TelegramBotClient(token);
                    var v = await bot.SendTextMessageAsync(idUser, text);
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
