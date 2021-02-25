using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control.CurrentBot;
using System.Data.SQLite;

namespace Goal_Achievement_Control.Tests
{

    [TestClass]
    public class UserTests
    {
        public DataBase db = new DataBase("TestDB");
        public Telegram.Bot.Types.Message message = new Telegram.Bot.Types.Message();
        int id = 1;
        User user = new User(new DataBase("TestDB"), idCurrentUser: 1, new Telegram.Bot.Types.Message());
        DataBaseTests DBTests = new DataBaseTests();

        [TestMethod]
        public void AddGoal_TestGoal_TestGoalReturned()
        {
            string execute = "TestGoal";
            string actual = "";

            try
            {
                user.AddGoal("TestGoal");
                foreach (var v in db.GetGoals(1))
                {
                    actual += v.Value;
                }
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }
            Assert.AreEqual(execute, actual);
        }
        [TestMethod]
        public void AddGoal_EmptyString_SpecialStringReturned()
        {
            string execute = "Строка пустая. Цель не добавлена";
            string actual = null;

            try
            {
                actual = user.AddGoal("   ");                
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }
            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void GoalsToString_nullGoals()
        {
            string execute = "Нет целей";
            string actual = "";
            
            actual = user.GoalsToString();            
            
            Assert.AreEqual(execute, actual);
        }

        /// <summary>
        /// tempUser создается т.к. в конструкторе User сразу инициализируется поле goals и оказывается пустым в объекте user;
        /// </summary>
        [TestMethod]
        public void GoalsToString_TestGoal_TestGoalReturned()
        {
            string execute = "TestGoal\n";
            string actual = "";
            try
            {                
                user.AddGoal("TestGoal");
                User tempUser = new User(db, id, message);
                actual = tempUser.GoalsToString();
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }
            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void DeleteGoal_1_0Returned()
        {
            string execute = "Нет целей";
            string actual = "empty";
            try
            {
                user.AddGoal("TestGoal");
                user.DeleteGoal(1);
                actual = user.GoalsToString();
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }

            Assert.AreEqual(execute, actual); 
        }

        [TestMethod]
        public void DeleteGoal_0_stringReturned()
        {
            string execute = "Указанный порядковый номер не соответствует порядковому номеру какой-либо цели";
            string actual = "empty";
            try
            {
                user.AddGoal("TestGoal");
                actual = user.DeleteGoal(1);                
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CountGoals_1Retuurned()
        {
            int execute = 1;
            int actual = -1;
            try
            {
                user.AddGoal("TestGoal");
                actual = user.CountGoals();
            }
            finally
            {
                DBTests.ClearTable("Goals");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void AddMarks_5_5Returned()
        {
            string execute = "5";
            string  actual = "";
            try
            {
                db.AddUser("1", "1", OperatingMode.NON);
                user.AddGoal("TestGoal");
                User tempUser = new User(db, id, message);
                tempUser.AddMarks("5");

                using (var connected = new SQLiteConnection ($"Data Source = {db.NameDataBase}"))
                {
                    connected.Open();
                    using (var cmd = connected.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT mark FROM MARKS WHERE goal_id == 1";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                actual += reader["mark"];
                            }
                        }
                    }
                }
            }
            finally
            {
                DBTests.ClearTable("Goals");
                DBTests.ClearTable("Marks");
                DBTests.ClearTable("Users");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void AddOperatingMode_Add_Addreturned()
        {
            OperatingMode execute = OperatingMode.AddGoal;
            OperatingMode actual = default;

            user.Mode = OperatingMode.AddGoal;

           actual = user.Mode;

            Assert.AreEqual(execute, actual);
        }
    }    
}
