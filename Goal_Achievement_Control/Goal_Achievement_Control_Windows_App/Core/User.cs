﻿/*
 * Класс содержит список целей, ID клиента, последнее сообщение и обработчик сообщений.
 * */

using System.Collections.Generic;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using Goal_Achievement_Control_Windows_App.Interfaces;

namespace Goal_Achievement_Control_Windows_App.Core 
{
    public class User : IUser
    {
        private OperatingMode mode;
        public OperatingMode Mode { 
            get => mode;
            set
            { 
                mode = AddOperatingMode(value);
            }
        }
        private Telegram.Bot.Types.Message message;

        /// <summary>
        /// set is:
        /// 1) set value to message 
        /// 2) send message to messageHandler.RateTypeMessage(Message)
        /// 3) get text from messageHandler.RateTypeMessage(Message)
        /// </summary>

        public Telegram.Bot.Types.Message Message
        {
            get => message;
            set     
            {
                //message = value;
                message.Text = messageHandler.RateTypeMessage(Message);
            }
        }
        private Dictionary<int, string> goals;  
        private IDataBase dataBase;
        public IDataBase DataBase
        {
            get => dataBase;
        }
        private int id; //ID в базе данных приложения
        public int ID
        {
            get => id;
            set => id = value;
        }
        public IInputMessageHandler messageHandler;  



        public User(IDataBase db, int idCurrentUser, Telegram.Bot.Types.Message mes, OperatingMode mode = OperatingMode.NON)
        {            
            messageHandler = new InputMessageHandler(this);
            dataBase = db;
            Mode = mode;
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
                return temp ?? "Нет целей";                                                       
        }

        public string AddGoal (string goal)
        {
            if (!string.IsNullOrWhiteSpace(goal))
            {
                dataBase.AddGoal(goal, ID);
                return "Цель добавлена";
            }
            else
            {
                return "Строка пустая. Цель не добавлена";
            }
            
        }
        
        public string DeleteGoal (int goalIndex)
        {            
            if (goalIndex <= goals.Count && goalIndex > 0)
            {
                DataBase.DeleteGoal(ID, goalIndex);
                return "Цель удалена";
            }
            else
            {
                return "Указанный порядковый номер не соответствует порядковому номеру какой-либо цели";
            }
            
        }

        public int CountGoals () => DataBase.CountGoals(ID);

        public string AddMarks (string newMarks)
        {
            string [] marks = newMarks.Replace(" ", "").Split(',');

            List<int> goals = new List<int>(dataBase.GetGoals(ID).Keys);

            if (marks.Length != goals.Count)
            {
                return "Разное количество оценок и целей. Повторите ввод оценок";
            }
            foreach (var v in marks)
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

            dataBase.AddMarks(ID, marks, goals);
            dataBase.ChangeOperatingMode(ID, OperatingMode.NON);
            return "Оценки добавлены";
        }

        private OperatingMode AddOperatingMode (OperatingMode om) => dataBase.AddOperatingMode(ID, om);

        public string GetMarks4Weaks() => dataBase.MarksLastFourWeeks(ID);

        public string GetAllMarks() => dataBase.MarksAll(ID);
    }
}
