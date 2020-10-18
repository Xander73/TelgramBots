/*
 * Класс содержит список целей, ID клиента, последнее сообщение и обработчик сообщений.
 * */

using Goal_Achievement_Control.Helpers;
using System;
using System.Collections.Generic;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Helpers
{

    public enum OperatingMode {AddGoal, DeleteGoal, NON};

    class Client    //client of bot
    {
        private OperatingMode mode;     //режим работы бота, добавление целей, удаление целей, обычный (NON).
        public OperatingMode Mode
        { 
            get => mode;
            set => mode = value;
        }

        public Client(Telegram.Bot.Types.Message message)
        {
            Mode = OperatingMode.NON;
            this.message = message;
            goals = new List<Goal>(15); //15 is max goals
            id = message.From.Id;
            messageHandler = new InputMessageHandler(this);
        }

        private Telegram.Bot.Types.Message message;
        public Telegram.Bot.Types.Message Message
        {
            get => message;
            set
            {
                message = value;
                messageHandler.RateTypeMessage(Message);
            }

        }
        public InputMessageHandler messageHandler;

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
                    if (targetindex - 1 >= 0 && targetindex - 1 < goals.Count)
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

        public List<Goal> Goals
        {
            get
            {
                return goals;
            }
        }   //получение доступа к списку goals;

        private int id;
        public int ID
        {
            get => id;
            set => id = value;
        }

        public string AddGoal (string goal)
        {
            Goal = new Goal (goal);
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
