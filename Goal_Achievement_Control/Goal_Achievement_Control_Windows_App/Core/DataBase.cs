using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;

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
                [user_id] integer not null"
                    );

            AddTable("Marks",
                @"[id] integer not null primary key autoincrement,
                  [mark] nvarchar(3) null,
                  [goal_id] integer not null"
                    );
        }

        private string nameDataBase;

        public void AddData (string nameTable, string data)     //формат строки data - "первый столбец id(его не пишем и начинаем со второго столбца), второй столбец, третий, ..."
        {            
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {                    
                    cmd.CommandText = $"insert into {nameTable} values({NextId(nameTable)}, {data})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddUser(string telegramId)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"insert into User values({NextId("User")}, {telegramId})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddGoal (string telegramId, string goal)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"insert into Goals values({NextId("Goals")}, {telegramId})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int NextId(string nameTable)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {

                    //определение текущего ID                    
                    cmd.CommandText = $"select max(id) from {nameTable}";
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
                    cmd.CommandText = $"select id from Users where telegramId == {telegramId}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return int.TryParse(reader[0].ToString(), out int res) ? res : 0;                        
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
                    cmd.CommandText = $@"create table if not exists [{nameTable}]({columns});";
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
                    cmd.CommandText = @"create table if not exists [TestTable](
                                [id] integer not null primary key autoincrement,
                                [value] nvarchar(2048) null
                            );";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into TestTable (value) values(:value)";
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
