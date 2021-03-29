using System;
using System.Collections.Generic;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Interfaces
{
    public interface IInputMessageHandler
    {
        string RateTypeMessage(Telegram.Bot.Types.Message message);

        string CommandHandler(string command);

        string TextHandler(string text);
    }
}
