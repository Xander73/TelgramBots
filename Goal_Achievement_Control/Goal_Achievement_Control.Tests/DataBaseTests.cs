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
        [TestMethod]
        public void AddTable_nameTableAndColumsName_TestTableRreterned()
        {
            DataBase db = new DataBase("TestDB");
            string nameTable = "TestTable";
            string arguments = "ID, Name";
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
                    //cmd.CommandText = "drop table testTable";
                    //cmd.ExecuteNonQuery();
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
