/*
 * Класс содержит список целей, ID клиента, последнее сообщение и обработчик сообщений.
 * */

using Goal_Achievement_Control.Helpers;
using System;
using System.Collections.Generic;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System.Text;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control.CurrentBot;

namespace Goal_Achievement_Control_Windows_App.Helpers
{
    
    class User    
    {
        private OperatingMode mode;     //режим работы бота, добавление целей, удаление целей, обычный (NON).
        public OperatingMode Mode {get; set;}
        private Telegram.Bot.Types.Message message;
        public Telegram.Bot.Types.Message Message
        {
            get => message;
            set     //устанавливает значение ьуыыфпу, обрабатывает входящее сообщение и присвает результат для дальнейшего вывода в сообщении пользователю.
            {
                message = value;
                message.Text = messageHandler.RateTypeMessage(Message); 
            }

        }
        public InputMessageHandler messageHandler;

        DataBase dataBase;

        public User(DataBase db, int idCurrentUser, Telegram.Bot.Types.Message mes, OperatingMode mode = OperatingMode.NON)
        {
            messageHandler = new InputMessageHandler(this);
            dataBase = db;
            ID = idCurrentUser;
            message = mes;  //не используется свойство, т.к. он начинает автоматически обрабатывать входящий текст
            dataBase.GetUserMod(ID);
        }
        

        public Goal Goal
        {
            //private get
            //{
            //    for (int i = 0; i < goals.Count; ++i)
            //    {
            //        Console.WriteLine($"{i+1}: {goals[i].Name};");
            //    }
            //    Console.WriteLine("Enter the target or the index");

            //    string searchingTarget = Console.ReadLine().ToLower();
            //    if (int.TryParse(searchingTarget, out int targetindex))
            //    {
            //        if (targetindex - 1 >= 0 && targetindex - 1 < goals.Count)
            //        {
            //            return goals[targetindex];
            //        }
            //        else
            //        {
            //            Console.WriteLine("Index is wrong");
            //            return null;
            //        }
            //    }
            //    else
            //    {
            //        foreach (var v in goals)
            //        {
            //            if (v.Name == searchingTarget)
            //            {
            //                return v;
            //            }
            //            else
            //            {
            //                Console.WriteLine("The target not found.");
            //            }
            //        }
            //    }

            //    Console.WriteLine("The name is not finde");
            //    return null;    //if not index and not finde a Name of target
            //}
            set
            {
                if (goals.Count < 15)
                {
                    goals.Add(value);
                }
                else
                {
                    Console.WriteLine("To much goals");
                }
            }
        }
                
        private int id;
        public int ID
        {
            get => id;
            set => id = value;
        }

        public string AddGoal (string goal)
        {
            
            return "Цель добавлена";
        }

        public string DeleteGoal (string goal)
        {
            foreach (var g in Goals)
            {
                if (g.Name == goal)
                    Goals.Remove(g);
                return "Цель удалена";
            }
            return "Цель не найдена";

            
        }

        
    }
}
