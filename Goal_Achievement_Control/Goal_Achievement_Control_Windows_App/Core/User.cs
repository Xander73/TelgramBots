/*
 * Класс содержит список целей, ID клиента, последнее сообщение и обработчик сообщений.
 * */

using System;
using System.Collections.Generic;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System.Text;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control.CurrentBot;
using System.Data.SQLite;
using System.Data;
using System.Security.Principal;
using Goal_Achievement_Control_Windows_App.Interfaces;

namespace Goal_Achievement_Control_Windows_App.Helpers 
{
    class User : IUser
    {
        public OperatingMode Mode { get; set; }
        private Telegram.Bot.Types.Message message;
        public Telegram.Bot.Types.Message Message
        {
            get => message;
            set     //устанавливает значение message, обрабатывает входящее сообщение и присвает результат для дальнейшего вывода в сообщении пользователю.
            {
                message = value;
                message.Text = messageHandler.RateTypeMessage(Message);
            }
        }
        private Dictionary<int, string> goals;
        private DataBase dataBase;
        public DataBase DataBase
        {
            get => dataBase;
        }
        private int id; //ID в базе данных приложения
        public int ID
        {
            get => id;
            set => id = value;
        }
        public InputMessageHandler messageHandler;



        public User(DataBase db, int idCurrentUser, Telegram.Bot.Types.Message mes, OperatingMode mode = OperatingMode.NON)
        {
            messageHandler = new InputMessageHandler(this);
            dataBase = db;
            ID = idCurrentUser; //ID в базе данных
            message = mes;  //не используется свойство, т.к. оно начинает автоматически обрабатывать входящий текст
            goals = dataBase.GetGoals(ID);
        }

        public string GoalsToString ()
        {
                string temp = null;
                foreach (var v in goals)    //DataBase.GetGoals(ID) возвращает Dictionary<int, string>, для вывода целей 
                {
                    temp += v.Value + '\n';     //выведем значения и объединим их в одну строку
                }
                return temp;                                                       
        }

        public string AddGoal (string goal)
        {
            dataBase.AddGoal(goal, ID);
            return "Цель добавлена";
        }
        
        public string DeleteGoal (int goalIndex)
        {
            DataBase.DeleteGoal(ID, goalIndex - 1);
            return "Цель удалена";
        }

        public int CountGoals ()
        {
            using (var connection = new SQLiteConnection ($"Data Sourse = {dataBase.NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT Count (Goal) FROM Goals WHERE {ID} == userId";
                    cmd.CommandType = CommandType.Text;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public string AddMarks (string text)
        {
            string [] marks = text.Replace(" ", "").Split(',');

            //List<int> goals = new List<int>(dataBase.GetGoals(ID).Keys);

           // if (marks.Length != goals.Count)
            {
                return "Разное количество оценок и целей. Повторите ввод оценок";
            }

#pragma warning disable CS0162 // Обнаружен недостижимый код
            foreach (var v in marks)
#pragma warning restore CS0162 // Обнаружен недостижимый код
            {
                if (!int.TryParse(v, out int res))
                {
                    return "Ошибка введенных данных. Не все оценки цифры. Повторите ввод оценок.";
                }
                else if (res < 0 || res > 10)
                {
                    return "Ошибка введенных данных. Не все оценки находтся в диапазоне от 0 до 10. Повторите ввод оценок.";
                }
            }

            //dataBase.AddMarks(ID, marks, goals);
            dataBase.ChangeOperatingMode(ID, OperatingMode.NON);
            return "Оценки добавлены";
        }
    }
}
