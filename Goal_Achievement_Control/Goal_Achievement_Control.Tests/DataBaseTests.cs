using Microsoft.VisualStudio.TestTools.UnitTesting;
using Goal_Achievement_Control_Windows_App;
using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Goal_Achievement_Control.Tests
{
    [TestClass]
    public class DataBaseTests
    {
        private DataBase db = new DataBase("TestDB");
        
        [TestMethod]
        public void AddTable_TestTable_TestTableRreterned()
        {            
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
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'";
                    using (var read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            actual = read["name"].ToString();
                            if (read["name"].ToString().Equals(expected))
                                actual = read["name"].ToString();
                        }
                    }                    
                }
            }
            Delete_TestTable(nameTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddData_1AndAlex_1AlexRetyrned ()
        {
            string nameTable = "TableForTests";
            string columns = @"[ID] integer not null primary key autoincrement, 
                               [Name] nvarchar (50) not null";
            string data = "Alex";
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
                    cmd.CommandText = $"DELETE FROM {nameTable}";
                }
            }

            Assert.AreEqual(expected, actual);
        }

        public void Delete_TestTable (string nameTable)
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
        

        

    }
}
