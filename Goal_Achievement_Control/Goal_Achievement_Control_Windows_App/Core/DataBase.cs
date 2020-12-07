using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using Goal_Achievement_Control.CurrentBot;
using System.Security.Cryptography;
using System.Data.SqlClient;
using Goal_Achievement_Control_Windows_App.Helpers;

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
                [telegramId] nvarchar(50) not null,
                [chatId] nvarchar(50) not null,
                [operatingMode] nvarchar(50) not null"
                                );
            //----- добавил столбец
            AddTable("Goals",
                @"[id] integer not null primary key autoincrement,
                [Goal] nvarchar(250) null,
                [userId] integer not null,
                [isMarked] bool not null"
                    );

            AddTable("Marks",
                @"[id] integer not null primary key autoincrement,
                  [Date] nvarchar(15) not null,
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

        public void AddUser(string telegramId, string cahtId, OperatingMode mode)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Users VALUES({NextId("Users")}, '{telegramId}', '{cahtId}', '{mode}')";
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
                    cmd.CommandText = $"INSERT INTO Goals VALUES({NextId("Goals")}, '{goal}', {userId}, false)";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddMarks(int userId, string[] marks, List<int> goalsId)
        {
            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    for (int i = 0; i < goalsId.Count; ++i)
                    {
                        cmd.CommandText = $@"INSERT INTO Marks VALUES ({NextId("Marks")}, '{DateTime.Now.Date.ToString()}', '{marks[i]}', {goalsId[i]})";
                        cmd.ExecuteNonQuery();
                    }
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
        }       //следующий id для ввола строки в базу данных

        public int IdCurrentUser (int telegramId)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {

                    //определение текущего ID                    
                    cmd.CommandText = $"SELECT id FROM Users WHERE telegramId == '{telegramId.ToString()}'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return int.TryParse(reader["id"].ToString(), out int result) ? result : 0;
                        }
                        return 0;                                                
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

        public void ChangeOperatingMode (int userId, OperatingMode mode)
        {
            using (var connection = new SQLiteConnection($"Data Sourse = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE Users SET operatingMode = '{mode}' WHERE id = {userId}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<string, string> Users
        {
            get
            {
                using (var Connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
                {
                    Connection.Open();
                    using (var cmd = Connection.CreateCommand())
                    {
                        Dictionary<string, string> telegramIdUsers = new Dictionary<string, string>();
                        cmd.CommandText = "SELECT telegramId, id from Users";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                telegramIdUsers.Add(reader["telegramId"].ToString(), reader["id"].ToString());
                            }
                            return telegramIdUsers;
                        }
                    }
                }
            }
        }

        public Dictionary<int, string> GetGoals(int userId)
        {
            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT id, Goal FROM Goals WHERE userId == {userId}";
                    cmd.ExecuteNonQuery();

                    Dictionary<int, string> resultate = new Dictionary<int, string>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        for (int i = 1; reader.Read(); ++i)
                        {
                            resultate.Add((int)reader["id"], $"{i}. {reader["Goal"]}\n");
                        }
                        return resultate;
                    }
                }
            }
        }

        public string DeleteGoal (int userId, int goalIndex)
        {
            List<string> goals = new List<string>( GetGoals(userId).Values);

            using (var connection = new SqlConnection ($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM Goals WHERE userId == {userId} AND Goal == '{goals[goalIndex-1]}'";
                    return goals[goalIndex - 1] + " удалена";
                }
            }
        }

        public string MarksLastFourWeeks(int userId)    //ID в базе данных приложения
        {
            Dictionary<int, string> goalsCurentUser = GetGoals(userId);
            List<Pair<DateTime, int>> dateMarks = new List<Pair<DateTime, int>>();
            List<Pair<string, double>> AVGMarks = new List<Pair<string, double>>();
            string resultate = null;

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    List<int> idGoals = new List<int> (goalsCurentUser.Keys);
                    for (int i = 0; i < goalsCurentUser.Count - 1; i++)
                    {
                        
                        string tempResultate = goalsCurentUser[i].ToString() + "\n\n";
                        cmd.CommandText = $"SELECT telegramId, Goal, Date, mark FROM Users " +
                            $"JOIN Goals ON Users.id == Goals.userId " +
                            $"JOIN Marks ON Goals.id == Marks.goal_id " +
                            $"WHERE Goals.id == '{idGoals[i]}' " +
                            $"ORDER BY Goals.id DESC " +
                            $"LIMIT 28";        //28 дней = 4 недели
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dateMarks.Add(new Pair<DateTime, int>((DateTime)reader["Date"], (int)reader["mark"]));
                                if ((dateMarks.Count % 7 == 0 && dateMarks.Count != 0) || DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                                {
                                    AVGMarks.Add(new Pair<string, double>($"Average weekly score:\nfrom {dateMarks[0].First} to {dateMarks[dateMarks.Count - 1].First}", CalculatingAVGMark(dateMarks)));
                                    dateMarks.Clear();
                                }
                                tempResultate += $"{reader["Date"]} - {reader["mark"]}\n";
                            }
                        }
                        resultate += tempResultate + "______________________________________________\n\n";

                        if (AVGMarks.Count == 0)
                        {
                            Console.WriteLine("Вы недавно начали движение к цели. ");
                        }
                        foreach (var v in AVGMarks)
                        {
                            Console.WriteLine($"{v.First} - {v.Second};");
                        }

                    }
                    return resultate;
                }
            }
        }

        public string MarksAll(int userId)
        {
            Dictionary<int, string> goalsCurentUser = GetGoals(userId);
            List<Pair<DateTime, int>> dateMarksWeek = new List<Pair<DateTime, int>>();
            List<Pair<string, double>> AVGMarksWeeks = new List<Pair<string, double>>();
            List<Pair<DateTime, int>> dateMarksAll = new List<Pair<DateTime, int>>();
            List<Pair<string, double>> AVGMarksMonths = new List<Pair<string, double>>();
            string resultate = null;

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    List<int> idGoals = new List<int>(goalsCurentUser.Keys);
                    for (int i = 0; i < goalsCurentUser.Count - 1; i++)
                    {

                        string tempResultate = goalsCurentUser[i].ToString() + "\n\n";
                        cmd.CommandText = $"SELECT telegramId, Goal, Date, mark FROM Users " +
                            $"JOIN Goals ON Users.id == Goals.userId " +
                            $"JOIN Marks ON Goals.id == Marks.goal_id " +
                            $"WHERE Goals.id == '{idGoals[i]}' " +
                            $"ORDER BY Goals.id DESC";        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dateMarksWeek.Add(new Pair<DateTime, int>((DateTime)reader["Date"], (int)reader["mark"])); 
                                if (dateMarksWeek.Count % 7 == 0 && dateMarksWeek.Count != 0)
                                {
                                    AVGMarksWeeks.Add(new Pair<string, double>($"Average weekly score:\nfrom {dateMarksWeek[0].First} to {dateMarksWeek[dateMarksWeek.Count - 1].First}", CalculatingAVGMark(dateMarksWeek)));
                                    dateMarksWeek.Clear();
                                }
                                dateMarksAll.Add(new Pair<DateTime, int>((DateTime)reader["Date"], (int)reader["mark"]));

                                tempResultate += $"{reader["Date"]} - {reader["mark"]}\n";                                
                            }
                        }
                        resultate += tempResultate + "______________________________________________\n\n";

                        foreach (var v in AVGMarksWeeks)
                        {
                            Console.WriteLine($"{v.First} - {v.Second};");
                        }
                    }
                    return resultate;
                }
            }
        }
    

        #region support_functions
        private double CalculatingAVGMark (List<Pair<DateTime, int>> datesMarks)
        {
            double markAVG = 0;
            foreach (var dateMark in datesMarks)
            {
                markAVG += dateMark.Second;
            }
            return markAVG /= datesMarks.Count; 

            //DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);

            //int weeks = datesMarks.Count / 7;   //na
        }

        private List<Pair<string, double>> CalculatingAVGMarkMonth(List<Pair<DateTime, int>> datesMarks)
        {
            List<Pair<string, double>> resultateAVGMarks = new List<Pair<string, double>>();
            DateTime monthDayFirst = default (DateTime);
            DateTime monthDayLast = default(DateTime);
            for (int i = 0; i<datesMarks.Count; ++i)
            {

            }
            if(monthDayFirst == default (DateTime))
            {
                monthDayFirst = datesMarks[0].First;                
            }
            else
            {

            }
            monthDayLast = datesMarks[0].First.AddMonths(1).AddDays(-1);
        }

        private class Pair<T, V>
        {
            public Pair (T first, V second)
            {
                First = first;
                Second = second;
            }
            public T First { get; set; }
            public V Second { get; set; }
        }
        #endregion

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
