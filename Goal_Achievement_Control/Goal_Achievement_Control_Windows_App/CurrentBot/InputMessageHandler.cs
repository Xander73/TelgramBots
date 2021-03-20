using System;
using System.Collections.Generic;
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
                        return "Введено максиальное количество целей.\nДля удаления цели введите команду - \"/Удалить цель\".";
                    }
                }
                else
                {
                    user.Mode = OperatingMode.AddGoal;        //режим ввода целей
                    return "Введите по порядук по одной цели. Должно быть от 3х до 15 целей.\nРежим редактирования целей открыт.";                    
                }
            }
            return "Неизвестная команда.";
        }

        public override string TextHandler(string text)
        {
            if (text == "Добавить цель")
            {
                if (user.CountGoals() < 15)
                {
                    user.Mode = OperatingMode.AddGoal;      //в следующем сообщении программа ожидает цель
                    return "Режим редактирования целей открыт.";
                }
                else
                {
                    return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель\".";
                }
            }
            else if (text == "Остановить ввод целей")
            {
                user.Mode = OperatingMode.NON;    //OperatingMode.NON - нет режима работы бота
                return "Режим редактирования целей закрыт.";
            }
            else if (text == "Удалить цель")
            {
                user.Mode = OperatingMode.DeleteGoal;
                return $"Режим удаления целей открыт.\n{ListGoalsToString()}\nВведите номер цели, которую требуется удалить.";
            }
            else if (text == "Список целей")  //вывести список целей
            {
                return ListGoalsToString();
            }
            else if (text == "Ввести оценки")
            {
                user.Mode = OperatingMode.AddMark;
                return $"Режим ввода оценок открыт.\n{ListGoalsToString()}\nВведите через запятую оценки для каждой цели по порядку. Оценки должны быть от 0 до 10.";
            }
            else if (text == "Статистика за 4 недели")
            {
                return user.GetMarks4Weaks();
            }
            else if (text == "Вся статистика")
            {
                return user.DataBase.MarksAll(user.ID);
            }

            if (user.Mode == OperatingMode.AddGoal)
            {
                int countGoals = user.CountGoals();
                if (countGoals < 15)     
                {                        
                    return user.AddGoal(text);                    
                }
                else
                {
                    user.Mode = OperatingMode.NON;

                    return "Введено максиальное количество целей.\nБот вышел из режима редактирования целей.\n";
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
            return "Неизвестный тип сообщения.";
        }

        public string ListGoalsToString()
        {
            string temp = "";
            List<string> goals = new List<string>();
            foreach (var v in user.DataBase.GetGoals(user.ID).Values)
            {
                goals.Add(v);
            }

            if (goals.Count == 0)
            {
                return "Нет целей.";
            }

            for (int i = 0; i < goals.Count; ++i)
            {
                temp += $"{i+1}) {goals[i]}\n";
            }
            return temp;
        }
    }
}
