﻿using System;
using Goal_Achievement_Control.CurrentBot;
using Goal_Achievement_Control_Windows_App.Core;

namespace Goal_Achievement_Control_Windows_App.CurrentBot
{
    public class InputMessageHandler : BaseInputMessageHandler
    {
        //TypeInputMessage typeMessage;
        User user;
        public InputMessageHandler(User user)
        {
            this.user = user;
        }

        public override string CommandHandler(string commandText)
        {
            if (commandText.ToLower () == "/вперед" || commandText.ToLower() == "/start")
            {

                if (user.CountGoals() > 3)
                {
                    if (user.CountGoals() < 15)
                    {
                        return "Вы уже начали путь к достижению цели. Необходимо минимум 3 цели.\nЧтобы добавить еще одну цель, введите команду \"/Добаить цель\".";
                    } 
                    else
                    {
                        return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                    }
                }
                else
                {
                    user.Mode = OperatingMode.AddGoal;        //режим ввода целей
                    return "Введите по одной цели./nРежим редактирования целей открыт.";                    
                }
            }
            else if (commandText.ToLower () == "/добавить цель")
            {
                if (user.CountGoals() < 15)
                {
                    user.Mode = OperatingMode.AddGoal;      //в следующем сообщении программа ожидает цель
                    return "Режим редактирования целей открыт.";
                }    
                else
                {
                    return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                }
            }
            else if (commandText.ToLower () == "/остановить ввод целей")
            {
                user.Mode = OperatingMode.NON;    //OperatingMode.NON - нет режима работы бота
                return "Режим редактирования целей закрыт.";
            }
            else if (commandText.ToLower() == "/удалить")
            {
                user.Mode = OperatingMode.DeleteGoal;
                return $"Режим удаления целей открыт.\n{ListGoalsToString()}\nВведите номер цели, которую требуется удалить.";
            }
            else if (commandText.ToLower() == "/цели")  //вывести список целей
            {                
                return ListGoalsToString();
            }
            else if (commandText.ToLower() == "/ввести оценки")
            {
                user.Mode = OperatingMode.AddMark;
                return $"Режим ввода оценок открыт.\n{ListGoalsToString()}\nВведите через запятую оценки для каждой цели по порядку. Оценки должны быть от 0 до 10";
            }
            else if (commandText.ToLower() == "/статистика за 4 недели")
            {
                return user.DataBase.MarksLastFourWeeks(user.ID);
            }
            else if (commandText.ToLower() == "/статистика")
            {
                return user.DataBase.MarksAll(user.ID);
            }
            else 
            {
                return "Неизвестная команда";
            }
        }

        public override string TextHandler(string text)
        {
            if (user.Mode == OperatingMode.AddGoal)
            {
                int countGoals = user.CountGoals();
                if (countGoals < 15)     //
                {                        
                    return user.AddGoal(text);                    
                }
                else
                {
                    user.Mode = OperatingMode.NON;

                    string temp = $"Введено максиальное количество целей.\nБот вышел из режима редактирования целей.\nВведите номер цели, которую требуется удалить.\n";
                    

                    return temp;
                }
            }
            else if (user.Mode == OperatingMode.DeleteGoal)
            {
                user.Mode = OperatingMode.NON;
                return user.DeleteGoal(Convert.ToInt32(text));
            }

            else if (user.Mode == OperatingMode.AddMark)
            {
                return user.AddMarks(text);
            }

            user.Mode = OperatingMode.NON;
            return "Неизвестный тип сообщения.\nРежим работы переведен в начальный";
        }

        public string ListGoalsToString()
        {
            string temp = "";
            string[] goals = user.GoalsToString().Split('\n');

            if (goals[0] == "Нет целей")
            {
                return goals[0];
            }

            for (int i = 0; i < goals.Length; ++i)
            {
                temp += $"{i}) {goals[i]}\n";
            }
            return temp;
        }
    }
}
