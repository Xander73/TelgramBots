using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Core
{
    enum TypeInputMessage
    {
        Command, Text, NONE
    }
    abstract class InputMessageHandler
    {
        public InputMessageHandler (Telegram.Bot.Types.Message inputMessage)
        {

        }

        private TypeInputMessage RateTypeMessage (Telegram.Bot.Types.Message inputMessage)
        {
            if (inputMessage.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                string inputText = inputMessage.Text;   //чтобы не вызывать функцию несколько раз
                if (inputText[0] == '/')
                {
                    CommandHendler(inputText);
                    return TypeInputMessage.Command;
                }
                else
                {
                    TextHandler(inputText);
                    return TypeInputMessage.Text;
                }
            }
            else
            {
                return TypeInputMessage.NONE;
            }
        }

        public abstract void CommandHendler (string commandText);

        public abstract void TextHandler(string text);
    }
}
