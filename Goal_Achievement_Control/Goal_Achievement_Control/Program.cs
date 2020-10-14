/*
 * Chatbot for planning assistance.
 * 
 * */

using System;
using System.Drawing;

namespace Goal_Achievement_Control
{
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            ///  The main entry point for the application.
            /// </summary>        
            /// 

            MainBot.MainBot bot = new MainBot.MainBot();

            Console.WriteLine("Bot started.");
        }
    }
}
