using System;
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
        public InputMessageHandler(User client)
        {
            this.user = client;
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
                    return "Введите от 3 до 15 целей./nРежим редактирования открыт.";                    
                }
            }
            else if (commandText.ToLower () == "/добавить цель")
            {
                user.Mode = OperatingMode.AddGoal;
                return "Режим редактирования целей открыт.";
            }
            else if (commandText.ToLower () == "/остановить")
            {
                user.Mode = OperatingMode.NON;    //нет режима работы бота
                return "Режим редактирования закрыт.";
            }
            else if (commandText.ToLower() == "/удалить")
            {
                user.Mode = OperatingMode.DeleteGoal;
                return "Режим удаления открыт.\nВедите цель, которую требуется удалить.\nДля просмотра всех целей введите команду \"/Цели\".";
            }
            else if (commandText.ToLower() == "/цели")
            {
                string tempGoals = null ;
                foreach (var g in user.Goals)
                {
                    tempGoals += g + "\n";
                }
                return tempGoals;
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
                if (user.CountGoals() < 15)
                {
                    return user.AddGoal(text);
                }
                else
                {
                    return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                }
            }
            if (user.Mode == OperatingMode.DeleteGoal)
            {                
                return user.DeleteGoal(text);
            }
            return "Неизвестный тип сообщения";
        }
    }
}
