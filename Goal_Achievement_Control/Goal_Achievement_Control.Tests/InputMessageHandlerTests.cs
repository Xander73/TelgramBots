using Goal_Achievement_Control_Windows_App.CurrentBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;

namespace Goal_Achievement_Control.Tests
{
    [TestClass]
    public class InputMessageHandlerTests 
    {
        DataBase db = new DataBase("TestDB");
        InputMessageHandler imh = new InputMessageHandler(new User(new DataBase("TestDB"), 1, new Telegram.Bot.Types.Message()));
        [TestMethod]
        public void ListGoalsToString_nullGoals()
        {
            string execute = "";
            string actual = "Empty string";

            actual = imh.ListGoalsToString();

            Assert.AreEqual(execute, actual);
        }
    }
    
}