using Goal_Achievement_Control_Windows_App.CurrentBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control.CurrentBot;

namespace Goal_Achievement_Control.Tests
{
    [TestClass]
    public class InputMessageHandlerTests 
    {
        DataBase db = new DataBase("TestDB");
        User user = new User(new DataBase("TestDB"), 1, new Telegram.Bot.Types.Message());
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

        [TestMethod]
        public void RateTypeMessage_UnnownMessage()
        {
            string execute = "Неизвестный тип сообщения";
            string actual = "";

            actual = imh.RateTypeMessage(new Telegram.Bot.Types.Message ());

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void RateTypeMessage_UnnownCommand()
        {
            string execute = "Неизвестная команда";
            string actual = "";
            var message = new Telegram.Bot.Types.Message();
            message.Text = "/command";
            actual = imh.RateTypeMessage(message);

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CommandHandler_TestGoal_stringReturned()
        {
            string execute = "Цель добавлена";
            string actual = "";

            Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
            message.Text = "Test Goal.";
            InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.AddGoal));
            actual = imh2.RateTypeMessage(message);

            db.ClearAllTables();

            Assert.AreEqual(execute, actual);
        }
    }
    
}