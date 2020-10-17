using System;
using System.Collections.Generic;
using System.Text;
using Goal_Achievement_Control.CurrentBot;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control_Windows_App.Helpers;

namespace Goal_Achievement_Control_Windows_App.Core
{
    class InputMessageHandler : BaseInputMessageHandler
    {
        TypeInputMessage typeMessage;
        Client client;
        public InputMessageHandler(Client client)
        {
            this.client = client;
        }

        public override void CommandHandler(string commandText)
        {
            if (commandText.ToLower () == "/вперед" || commandText.ToLower() == "/start")
            {

            }
        }

        public override void TextHandler(string text)
        {
            
        }
    }
}
