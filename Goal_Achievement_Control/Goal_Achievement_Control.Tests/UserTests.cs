using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;

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

        /// <summary>
        /// tempUser создается т.к. в конструкторе User поле goals сразу инициализируется и пустое в объекте user;
        /// </summary>
        [TestMethod]
        public void GoalsToString_TestGoal_TestGoalReturned()
        {
            string execute = "TestGoal\n";
            string actual = "";
            try
            {                
                user.DataBase.AddGoal("TestGoal", 1);
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
    }    
}
