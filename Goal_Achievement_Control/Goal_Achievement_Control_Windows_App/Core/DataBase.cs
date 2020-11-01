using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using Goal_Achievement_Control.CurrentBot;
using System.Security.Cryptography;

namespace Goal_Achievement_Control_Windows_App.Core
{
    class DataBase
    {
        public DataBase(string nameDataBase)
        {
            this.nameDataBase = nameDataBase + ".db";
            if (!File.Exists(this.nameDataBase))
                SQLiteConnection.CreateFile(this.nameDataBase);

            AddTable("Users",
                @"[id] integer not null primary key autoincrement,
                [telegramId] integer not null,
                [operatingMode] nvarchar(50) not null"
                    );

            AddTable("Goals",
                @"[id] integer not null primary key autoincrement,
                [Goal] nvarchar(250) null,
                [userId] integer not null"
                    );

            AddTable("Marks",
                @"[id] integer not null primary key autoincrement,
                  [mark] nvarchar(3) null,
                  [goal_id] integer not null"
                    );
        }


        private string nameDataBase;
        public string NameDataBase { get => nameDataBase; }

        public void AddData (string nameTable, string data)     //формат строки data - "первый столбец id(его не пишем и начинаем со второго столбца), второй столбец, третий, ..."
        {            
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {                    
                    cmd.CommandText = $"INSERT INTO {nameTable} VALUES({NextId(nameTable)}, {data})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddUser(string telegramId, OperatingMode mode)
        {
            using (var connection = new SQLiteConnection($"Data Source=MyDB.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Users VALUES({NextId("Users")}, {telegramId}, '{mode}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddGoal(string goal, int userId)
        {
            using (var connection = new SQLiteConnection($"Data Source=MyDB.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Goals VALUES({NextId("Goals")}, '{goal}', {userId})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public OperatingMode GetUserMod(int id)
        {
            using (var connection = new SQLiteConnection($"Data Sourse = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT operatingMode FROM Users WHERE id = {id}";

                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        switch (reader["operatingMode"].ToString())
                        {
                            case "NON":
                                {
                                    return OperatingMode.NON;
                                }
                            case "AddGoal":
                                {
                                    return OperatingMode.AddGoal;
                                }
                            case "DeleteGoal":
                                {
                                    return OperatingMode.DeleteGoal;
                                }
                            default:
                                return OperatingMode.Error;
                        }
                    }
                }
            }
        }

        public int NextId(string nameTable)
        {
            using (var connection = new SQLiteConnection($"Data Source=MyDB.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {

                    //определение текущего ID                    
                    cmd.CommandText = $"SELECT MAX(id) FROM {nameTable}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return int.TryParse(reader[0].ToString(), out int res) ? (res + 1) : 1;
                    }
                }
            }
        }

        public int IdCurrentUser (int telegramId)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {

                    //определение текущего ID                    
                    cmd.CommandText = $"SELECT id FROM Users WHERE telegramId == {telegramId}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return int.TryParse(reader[0].ToString(), out int result) ? result : 0;                        
                    }
                }
            }
        }

        private void AddTable(string nameTable, string columns)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS [{nameTable}]({columns});";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        void temp()
        {
            if (!File.Exists("testDB.db"))
                SQLiteConnection.CreateFile("testDB.db");
            using (var connection = new SQLiteConnection("Data Source=testDB.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [TestTable](
                                [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [value] NVARCHAR(1000) NULL
                            );";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO TestTable (value) values(:value)";
                    cmd.Parameters.AddWithValue("value", "abc");
                    cmd.ExecuteNonQuery();
                    

                    cmd.CommandText = "select * from TestTable";
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            Console.WriteLine("Id={0}, Value={1}", reader["id"], reader["value"]);
                }
            }
        }
    }
}
