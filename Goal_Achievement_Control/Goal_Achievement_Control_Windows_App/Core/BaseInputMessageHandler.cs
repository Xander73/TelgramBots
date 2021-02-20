using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.CurrentBot
{
    //public enum TypeInputMessage
    //{
    //    Command, Text, NONE
    //}
    public abstract class BaseInputMessageHandler
    {
        public string RateTypeMessage (Telegram.Bot.Types.Message inputMessage)
        {
            if (inputMessage.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                string inputText = inputMessage.Text;   //чтобы не вызывать функцию несколько раз
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    return "Сообщение пустое";
                }
                if (inputText[0] == '/')
                {                    
                    return CommandHandler(inputText); ;
                }
                else
                {                    
                    return TextHandler(inputText); 
                }
            }
            else
            {
                return "Неизвестный тип сообщения";
            }
        }

        public abstract string CommandHandler (string commandText);

        public abstract string TextHandler(string text);
    }
}
