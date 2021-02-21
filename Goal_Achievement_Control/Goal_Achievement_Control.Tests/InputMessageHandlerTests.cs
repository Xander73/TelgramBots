using Goal_Achievement_Control_Windows_App.CurrentBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control.CurrentBot;
using System.Collections.Generic;

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

            db.ClearAllTables();

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

            db.ClearAllTables();

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void RateTypeMessage_EmptyString_stringReturned()
        {
            string execute = "Сообщение пустое";
            string actual = "";

            Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
            message.Text = "";
            InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.AddGoal));
            actual = imh2.RateTypeMessage(message);

            db.ClearAllTables();

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void TextHandler_OperatingModeAddTestGoal_stringReturned()
        {
            string execute = "Цель добавлена";
            string actual = "";

            Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
            message.Text = "Test Goal.";
            InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.AddGoal));
            actual = imh2.TextHandler("Test Goal.");

            db.ClearAllTables();

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void TextHandler_OperatingModeAddMore16TestGoals_stringReturned()
        {
            string execute = "Введено максиальное количество целей.\nБот вышел из режима редактирования целей.\n";
            string actual = "";

            for (int i =0; i < 15; ++i)
            {
                db.AddGoal($"TestGoal{i}", 1);
            }

            Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
            message.Text = "Test Goal.";
            InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.AddGoal));
            actual = imh2.TextHandler("TestGoal16");

            db.ClearAllTables();

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void TextHandler_OperatingModeDelete_Index1_stringReturned()
        {
            string execute = "Цель удалена";
            string actual = "";
            
            Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
            message.Text = "Test Goal.";
            try
            {
                for (int i = 0; i < 2; ++i)
                {
                    db.AddGoal($"TestGoal{i}", 1);
                }

                InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.DeleteGoal));
                actual = imh2.TextHandler("1");
            }
            finally
            {
                db.ClearAllTables();
            }                     

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void TextHandler_OperatingModeAddMark_1_stringReturned()
        {
            string execute = "Оценки добавлены";
            string actual = "";
                        
            try
            {
                db.AddUser("1", "1", OperatingMode.AddMark);
                db.AddGoal("TestGoal", 1);
                for (int i = 0; i < 2; ++i)
                {
                    db.AddMarks(1, new string[] {"1", "2" }, new List<int>() { 1 } );
                }
                InputMessageHandler imh2 = new InputMessageHandler(new User(db, 1, new Telegram.Bot.Types.Message(), OperatingMode.AddMark));
                actual = imh2.TextHandler("1");
            }
            finally
            {
                db.ClearAllTables();
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CommandHandler_Command_впередAnd0Goals_stringReturned()
        {
            string execute = "Введите по порядук по одной цели. Должно быть от 3х до 15 целей.\nРежим редактирования целей открыт.";
            string actual = "";

            try
            {                
                actual = imh.CommandHandler("/Вперед");
            }
            finally
            {
                db.ClearAllTables();
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CommandHandler_Command_впередAnd15Goals_stringReturned()
        {
            string execute = "Введено максиальное количество целей.\nДля удаления цели введите команду - \"/Удалить цель\".";
            string actual = "";
            const int MAX_GOALS = 15;

            try
            {
                for (int i = 0; i < MAX_GOALS; ++i)
                {
                    db.AddGoal($"{i+1}Goal", 1);
                }
                actual = imh.CommandHandler("/Вперед");
            }
            finally
            {
                db.ClearAllTables();
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CommandHandler_Command_впередAnd4Goals_stringReturned()
        {
            string execute = "Вы уже начали путь к достижению цели. Необходимо минимум 3 цели.\nЧтобы добавить еще одну цель, введите команду \"/Добаить цель\".";
            string actual = "";
            const int MAX_GOALS = 4;

            try
            {
                for (int i = 0; i < MAX_GOALS; ++i)
                {
                    db.AddGoal($"{i + 1}Goal", 1);
                }
                actual = imh.CommandHandler("/Вперед");
            }
            finally
            {
                db.ClearAllTables();
            }

            Assert.AreEqual(execute, actual);
        }
    }
    
}