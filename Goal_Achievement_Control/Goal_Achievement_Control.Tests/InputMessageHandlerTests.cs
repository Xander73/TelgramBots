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
            string execute = "Нет целей";
            string actual = "";

            actual = imh.ListGoalsToString();

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void ListGoalsToString_TestGoal_1TestGoal()
        {
            string execute = "1) TestGoal\n";
            string actual = "";
            try
            {
                db.AddGoal("TestGoal", 1);

                InputMessageHandler imh2 = new InputMessageHandler(new User(new DataBase("TestDB"), 1, new Telegram.Bot.Types.Message()));
                actual = imh2.ListGoalsToString();
            }
            finally
            {
                db.ClearTable("Goals");
            }                        

            Assert.AreEqual(execute, actual);
        }
    }
    
}