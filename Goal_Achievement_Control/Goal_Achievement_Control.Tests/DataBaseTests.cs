using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App.Core;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using Goal_Achievement_Control_Windows_App.CurrentBot;

namespace Goal_Achievement_Control.Tests
{
    [TestClass]
    public class DataBaseTests
    {
        private DataBase db = new DataBase("TestDB");

        [TestMethod]
        public void AddTable_TestTable_TestTableRreturned()
        {
            db.AddTable("Users",
                                @"[id] integer not null primary key autoincrement,
                [telegramId] nvarchar(50) not null,
                [chatId] nvarchar(50) not null,
                [operatingMode] nvarchar(50) not null"
                                    );

            db.AddTable("Goals",
                @"[id] integer not null primary key autoincrement,
                [Goal] nvarchar(250) null,
                [userId] integer not null,
                [isMarked] bool not null"
                    );

            db.AddTable("Marks",
                @"[id] integer not null primary key autoincrement,
                  [Date] nvarchar(15) not null,
                  [mark] nvarchar(3) null,
                  [goal_id] integer not null"
                    );

            string nameTable = "TestTable";
            string arguments = @"[ID] integer not null primary key autoincrement,
                                 [Name] nvarchar(50) not null";
            string expected = "TestTable";
            string actual = "";

            db.AddTable(nameTable, arguments);
            using (var connected = new SQLiteConnection($"Data Source={db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%' ORDER BY 'table'";
                    using (var read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            string temp = read["name"].ToString();
                            if (read["name"].ToString().Equals(expected))
                                actual = read["name"].ToString();
                        }
                    }
                }
            }
            Drop_TestTable(nameTable);
            //Delete_TestTable("Users");
            //Delete_TestTable("Goals");
            //Delete_TestTable("Marks");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddData_1AndAlex_1AlexReturned()
        {
            string nameTable = "TableForTests";
            string columns = @"[ID] integer not null primary key autoincrement, 
                               [Name] nvarchar (50) not null";
            string data = "'Alex'";
            string expected = "1Alex";
            string actual = "";

            db.AddTable(nameTable, columns);

            db.AddData(nameTable, data);

            using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = $"SELECT ID, Name FROM {nameTable}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actual += reader["ID"].ToString() + reader["Name"].ToString();
                        }
                    }
                }
            }
            ClearTable(nameTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddUser_1And1And1AndNON_111NONReturned()
        {
            string expected = "111NON";
            string actual = "";

            db.AddUser("1", "1", OperatingMode.NON);

            using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, telegramId, chatId, operatingMode FROM Users";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actual += reader["id"].ToString();
                            actual += reader["telegramId"];
                            actual += reader["chatId"];
                            actual += reader["operatingMode"];
                        }
                    }
                }
            }
            ClearTable("Users");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddGoal_TestGoalAnd1_TestGoal1Returned()
        {
            string expected = "1TestGoal1False";
            string actual = "";
            string testGoal = "TestGoal";
            int userId = 1;

            db.AddGoal(testGoal, userId);

            using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, Goal, userId, isMarked FROM Goals";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actual += reader["id"].ToString() + reader["Goal"] + reader["userId"] + reader["isMarked"].ToString();
                        }
                    }
                }
            }
            ClearTable("Goals");
            Assert.AreEqual(expected, actual); ;
        }

        [TestMethod]
        public void AddMarks_1AndCurrendDateAnd5And1_1currentDate51Returned()
        {
            string expected = "1" + DateTime.Now.ToShortDateString() + "51";
            string actual = "";
            int idUser = 1;
            string[] marks = { "5" };
            List<int> goalsId = new List<int>() { 1 };

            db.AddMarks(idUser, marks, goalsId);

            using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, Date, mark, goal_id FROM Marks";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actual += reader["id"].ToString() + reader["Date"] + reader["mark"] + reader["goal_id"];
                        }
                    }
                }
            }
            ClearTable("Marks");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddOperatingMode_OperatingModeAddGoal_AddGoalReturned()
        {
            OperatingMode execute = OperatingMode.AddGoal;
            OperatingMode actual = default;

            try
            {
                db.AddUser("1", "1", OperatingMode.NON); 
                actual = db.ChangeOperatingMode(1, OperatingMode.AddGoal);
            }
            finally
            {
                db.ClearTable("Users");
            }

            
        }

        [TestMethod]
        public void GetUserMod_NON_NONReturned()
        {
            OperatingMode execute = OperatingMode.NON;
            OperatingMode actual = default;
            db.AddUser("1", "1", OperatingMode.NON);

            actual = db.GetUserMod(1);
            ClearTable("Users");

            Assert.AreEqual(execute, actual);
        }

        /// <summary>
        /// NextId(int Id) ����� �������� �������� ��� ����������� ��������:
        /// 1. ������ ������� - ���������� 1;
        /// 2. ������� ��������� - ���������� �������� �� 1 ������ ������������� Id
        /// </summary>
        [TestMethod]
        public void NextId_EmptyTable_1Returned()
        {
            int execute = 1;
            int actual = 0;

            actual = db.NextId("Users");

            ClearTable("Users");
            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void NextId_NonEmptyTable_2Returned()
        {
            int execute = 2;
            int actual = 0;
            db.AddUser("1", "1", OperatingMode.NON);

            actual = db.NextId("Users");

            ClearTable("Users");
            Assert.AreEqual(execute, actual);
        }

        /// <summary>
        /// IdCurrentUser(int telegramId) ����� �������� �������� ��� ����������� ��������:
        /// 1. ��� ���������� - ���������� 0;
        /// 2. ���� ���������� - ���������� �������� �������� id � ���� ������ ����������
        /// </summary>
        [TestMethod]
        public void IdCurrentUser_10_0Returned()
        {
            int execute = 0;
            int actual = -1;

            actual = db.IdCurrentUser(10);

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void IdCurrentUser_1_1Returned()
        {
            int execute = 0;
            int actual = -1;
            db.AddUser("1", "1", OperatingMode.NON);

            actual = db.IdCurrentUser(10);

            ClearTable("Users");
            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void ChangeOperatingMode_Error_ErrorReturned()
        {
            OperatingMode execute = OperatingMode.Error;
            OperatingMode actual = default;
            db.AddUser("1", "1", OperatingMode.NON);

            db.ChangeOperatingMode(1, OperatingMode.Error);
            actual = db.GetUserMod(1);
            ClearTable("Users");

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void GetTelegramIdUsers_1and2_21Returned()
        {
            string execute = "21";
            string actual = null;
            db.AddUser("2", "1", OperatingMode.NON);
            string[] tempKeyValue = new string[2];
            try
            {
                var temp = db.GetTelegramIdUsers();
                temp.Keys.CopyTo(tempKeyValue, 0);
                temp.Values.CopyTo(tempKeyValue, 1);
                actual = tempKeyValue[0] + tempKeyValue[1];
            }

            finally
            {
                ClearTable("Users");
                Assert.AreEqual(execute, actual);
            }
        }

        /// <summary>
        /// method return string - "11. TestGoal\n"
        /// </summary>
        [TestMethod]
        public void GetGoals_1_11TestGoalReturned()
        {
            string execute = "1TestGoal";
            string actual = "";
            try
            {
                db.AddGoal("TestGoal", 1);

                var temp = db.GetGoals(1);
                string[] results = new string[temp.Count * 2];
                foreach (var v in temp)
                {
                    actual += v.Key.ToString() + v.Value;
                }
            }

            finally
            {
                ClearTable("Goals");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void DeleteGoal_TestGoalAnd1_0Returned()
        {
            int execute = 0;
            int actual = -1;

            try
            {
                db.AddGoal("TestGoal", 1);
                //db.AddGoal("TestGoal", 1);
                string s = db.DeleteGoal(1, 1);

                using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
                {
                    connected.Open();
                    using (var cmd = connected.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM Goals";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                actual = Convert.ToInt32(reader[0]);
                            }
                        }
                        cmd.CommandText = "SELECT id, Goal, userId, isMarked FROM Goals";
                        using (var reader = cmd.ExecuteReader())
                        {
                            string temp = "";
                            while (reader.Read())
                            {
                                temp = reader[0].ToString() + reader[1].ToString() + reader[2].ToString() + reader[3].ToString();
                            }
                        }
                    }
                }
            }
            finally
            {
                ClearTable("Goals");
            }
            Assert.AreEqual(execute, actual);
        }
        /// <summary>
        /// ���� � ����� ����������
        /// </summary>
        [TestMethod]
        public void MarksLastFourWeeks_0Marks_SpecialMessageReturned()
        {
            string execute = "Average weekly score:\n�� ������� ������ �������� � ����.\n������ ���.";
            string actual = "";

            try
            {
                //db.AddUser("1", "1", OperatingMode.NON);
                db.AddGoal("TestGoal", 1);
                //db.AddMarks(1, new string [] { "1" }, new List<int>(1));

                actual = db.MarksLastFourWeeks(1);
            }

            finally
            {
                ClearTable("Users");
                ClearTable("Goals");
                ClearTable("Marks");
            }


            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void MarksLastFourWeeks_40Marks_28Marksreturned()
        {
            string execute = $"TestGoal\n\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n{DateTime.Now.ToShortDateString()} - 1\n" +
                $"______________________________________________\n\n" +
                $"Average weekly score:\nfrom {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()} - 1;\n" +
                $"Average weekly score:\nfrom {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()} - 1;\n" +
                $"Average weekly score:\nfrom {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()} - 1;\n" +
                $"Average weekly score:\nfrom {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()} - 1;\n";
            string actual = "";

            try
            {
                db.AddUser("1", "1", OperatingMode.NON);
                db.AddGoal("TestGoal", 1); ;

                for (int i = 1; i <= 40; ++i)
                {
                    db.AddMarks(1, new string[] { "1" }, new List<int>() { 1 });
                }

                actual = db.MarksLastFourWeeks(1);
            }

            finally
            {
                ClearTable("Users");
                ClearTable("Goals");
                ClearTable("Marks");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void MarksAll_40Marks_40Marksreturned()
        {
            string execute = $"TestGoal\n\n" +

$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
$"Week from {DateTime.Now.ToShortDateString()} to {DateTime.Now.ToShortDateString()}: 1\n" +
"______________________________________________\n\n" +

$"Month - {DateTime.Now.ToString("MMMM", CultureInfo.CurrentCulture)}:  1\n\n";
            string actual = "";

            try
            {
                db.AddUser("1", "1", OperatingMode.NON);
                db.AddGoal("TestGoal", 1); 

                for (int i = 1; i <= 40; ++i)
                {
                    db.AddMarks(1, new string[] { "1" }, new List<int>() { 1 });
                }

                actual = db.MarksAll(1);
            }

            finally
            {
                ClearTable("Users");
                ClearTable("Goals");
                ClearTable("Marks");
            }

            Assert.AreEqual(execute, actual);
        }

        [TestMethod]
        public void CountGoals_1_2Returned()
        {
            int execute = 2;
            int actual = 0;
            try
            {
                for (int i = 0; i < 2; ++i)
                {
                    db.AddGoal($"TestGoal", 1);
                }

                actual = db.CountGoals(1);
            }

            finally
            {
                ClearTable("Goals");
            }

            Assert.AreEqual(execute, actual);

        }
                
        public void Drop_TestTable(string nameTable)
        {
            using (var connected = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = $"DROP TABLE {nameTable}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ClearTable(string nameTable)
        {
            using (var connacted = new SQLiteConnection($"Data Source = {db.NameDataBase}"))
            {
                connacted.Open();
                using (var cmd = connacted.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {nameTable}";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
