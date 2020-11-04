﻿using System;
using System.Collections.Generic;
using System.Text;
using Goal_Achievement_Control.CurrentBot;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using Goal_Achievement_Control_Windows_App.Helpers;

namespace Goal_Achievement_Control_Windows_App.CurrentBot
{
    class InputMessageHandler : BaseInputMessageHandler
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
                        return "Вы уже начали путь к достижению цели. Необходимо минимум 3 цели.\nЧтобы добавить еще одну цель введите команду \"/Добаить цель\".";
                    } 
                    else
                    {
                        return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                    }
                }
                else
                {
                    user.Mode = OperatingMode.AddGoal;        //режим ввода целей
                    return "Введите от 3 до 15 целей./nРежим редактирования целей открыт.";                    
                }
            }
            else if (commandText.ToLower () == "/добавить цель")
            {
                if (user.CountGoals() < 15)
                {
                    user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.AddGoal);
                    return "Режим редактирования целей открыт.";
                }    
                else
                {
                    return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                }
            }
            else if (commandText.ToLower () == "/остановить ввод целей")
            {
                user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.NON);    //нет режима работы бота
                return "Режим редактирования целей закрыт.";
            }
            //---------
            else if (commandText.ToLower() == "/удалить")
            {
                user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.DeleteGoal);
                return $"Режим удаления целей открыт.\n{user.Goals}\nВведите номер цели, которую требуется удалить.";
            }
            else if (commandText.ToLower() == "/цели")  //вывести список целей
            {                
                return user.Goals;
            }
            else if (commandText.ToLower() == "/ввести оценки")
            {
                user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.AddMark);
                return $"Режим ввода оценок открыт.\n{user.Goals}\nВведите через запятую оценки каждой цели по порядку";
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
                if (user.CountGoals() < 15)     //
                {
                    return user.AddGoal(text);
                }
                else
                {
                    user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.NON);
                    return $"Введено максиальное количество целей.\n{user.Goals}\nВведите номер цели, которую требуется удалить.\n";
                }
            }
            if (user.Mode == OperatingMode.DeleteGoal)
            {
                user.DataBase.ChangeOperatingMode(user.ID, OperatingMode.NON);
                return user.DeleteGoal(Convert.ToInt32(text));
            }
            return "Неизвестный тип сообщения";
        }
    }
}
