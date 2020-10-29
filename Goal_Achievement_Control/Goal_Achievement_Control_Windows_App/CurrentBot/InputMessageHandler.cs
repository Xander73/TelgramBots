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
        User client;
        public InputMessageHandler(User client)
        {
            this.client = client;
        }

        public override string CommandHandler(string commandText)
        {
            if (commandText.ToLower () == "/вперед" || commandText.ToLower() == "/start")
            {
                if (client.Goals.Count > 3)
                {
                    if (client.Goals.Count < 15)
                    {
                        return "Вы уже начали путь к достижению цели.\nЧтобы добавить еще одну цель введите команду \"/Добаить цель\".";
                    } 
                    else
                    {
                        return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                    }
                }
                else
                {
                    client.Mode = OperatingMode.AddGoal;        //режим ввода целей
                    return "Введите от 3 до 15 целей./nРежим редактирования открыт.";                    
                }
            }
            else if (commandText.ToLower () == "/добавить цель")
            {
                client.Mode = OperatingMode.AddGoal;
                return "Режим редактирования открыт.";
            }
            else if (commandText.ToLower () == "/остановить")
            {
                client.Mode = OperatingMode.NON;    //нет режима работы бота
                return "Режим редактирования закрыт.";
            }
            else if (commandText.ToLower() == "/удалить")
            {
                client.Mode = OperatingMode.DeleteGoal;
                return "Режим удаления открыт.\nВедите цель, которую требуется удалить.\nДля просмотра всех целей введите команду \"/Цели\".";
            }
            else if (commandText.ToLower() == "/цели")
            {
                string tempGoals = null ;
                foreach (var g in client.Goals)
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
            if (client.Mode == OperatingMode.AddGoal)
            {
                if (client.Goals.Count <= 15)
                {
                    return client.AddGoal(text);
                }
                else
                {
                    return "Введено максиальное количество целей.\nДля удаления цели введите команду \"/Удалить цель.\"";
                }
            }
            if (client.Mode == OperatingMode.DeleteGoal)
            {                
                return client.DeleteGoal(text);
            }
            return "Неизвестный тип сообщения";
        }
    }
}
