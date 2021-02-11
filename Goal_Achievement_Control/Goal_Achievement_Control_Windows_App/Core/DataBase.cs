using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Goal_Achievement_Control.CurrentBot;
using System.Data.SqlClient;
using Goal_Achievement_Control_Windows_App.Interfaces;

namespace Goal_Achievement_Control_Windows_App.Core
{
    public class DataBase : IDataBase
    {
        private string nameDataBase;
        public string NameDataBase { get => nameDataBase; }



        public DataBase(string nameDataBase)
        {
            this.nameDataBase = nameDataBase + ".db";
            if (!File.Exists(this.nameDataBase))
            {
                SQLiteConnection.CreateFile(this.nameDataBase);

                AddTable("Users",
                                @"[id] integer not null primary key autoincrement,
                [telegramId] nvarchar(50) not null,
                [chatId] nvarchar(50) not null,
                [operatingMode] nvarchar(50) not null"
                                    );

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
        }


        /// <summary>
        /// формат строки data - "первый столбец id(его не пишем и начинаем со второго столбца) 
        ///второй столбец, третий, ..."
        ///текстовые стрки пишем с одной кавычкой - 'текст'
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="data"></param>
        public void AddData(string nameTable, string data)     
        {
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
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
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
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
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
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
            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    for (int i = 0; i < goalsId.Count; ++i)
                    {
                        cmd.CommandText = $@"INSERT INTO Marks VALUES ({NextId("Marks")}, '{DateTime.Now.Date.ToShortDateString()}', '{marks[i]}', {goalsId[i]})";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public OperatingMode GetUserMod(int id)
        {
            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
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
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
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
        }       //следующий id для ввода строки в базу данных

        public int IdCurrentUser(int telegramId)
        {
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {

                    //определение id в базе данных
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

        public void AddTable(string nameTable, string columnsWithAttributes)
        {
            using (var connection = new SQLiteConnection($"Data Source={NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS [{nameTable}]({columnsWithAttributes});";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ChangeOperatingMode(int userId, OperatingMode mode)
        {
            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE Users SET operatingMode = '{mode}' WHERE id = {userId}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<string, string> GetTelegramIdUsers ()
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

        public Dictionary<int, string> GetGoals(int userId)
        {
            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
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
                            resultate.Add(Convert.ToInt32(reader["id"]), $"{i}. {reader["Goal"]}\n");
                        }
                        return resultate;
                    }
                }
            }
        }

        public string DeleteGoal(int userId, int goalIndex)
        {
            List<string> goals = new List<string>(GetGoals(userId).Values);

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM Goals WHERE userId == {userId} AND Goal == '{goals[goalIndex - 1]}'";
                    cmd.ExecuteNonQuery();
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
                    List<int> idGoals = new List<int>(goalsCurentUser.Keys);
                    for (int i = 0; i < goalsCurentUser.Count - 1; i++)
                    {

                        string tempResultate = goalsCurentUser[idGoals[i]].ToString() + "\n\n";
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
                            resultate += "Average weekly score:\nВы недавно начали движение к цели.\nОценок нет.";
                        }
                        else
                        {
                            foreach (var v in AVGMarks)
                            {
                                resultate += $"{v.First} - {v.Second};\n";
                            }
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

            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    List<int> idGoals = new List<int>(goalsCurentUser.Keys);
                    for (int i = 0; i < goalsCurentUser.Count - 1; i++)
                    {
                        string tempResultate = goalsCurentUser[idGoals[i]] + "\n\n";
                        cmd.CommandText = $"SELECT telegramId, Goal, Date, mark FROM Users " +
                            $"JOIN Goals ON Users.id == Goals.userId " +
                            $"JOIN Marks ON Goals.id == Marks.goal_id " +
                            $"WHERE Goals.id == '{idGoals[i]}' " +
                            $"ORDER BY Goals.id DESC";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                dateMarksAll.Add(new Pair<DateTime, int>((DateTime)reader["Date"], (int)reader["mark"]));
                            }
                        }
                        resultate += tempResultate + CalculatingAVGMarkWeekly(dateMarksAll) +
                            "______________________________________________\n\n" +
                            CalculatingAVGMarkMonthly(dateMarksAll);
                    }                        
                    return resultate;
                }
            }
        }


        #region support_functions

        private double CalculatingAVGMark(List<Pair<DateTime, int>> datesMarks)
        {
            double markAVG = 0;
            foreach (var dateMark in datesMarks)
            {
                markAVG += dateMark.Second;
            }
            return markAVG /= datesMarks.Count;
        }

        private List<Pair<string, double>> CalculatingAVGMarkWeekly(List<Pair<DateTime, int>> datesMarks)
        {
            List<Pair<string, double>> resultateAVGMarks = new List<Pair<string, double>>();
            int indexLastday = 0;

            if (datesMarks.Count == 0)
            {
                return new List<Pair<string, double>>();
            }

            for (int i = 0; i < datesMarks.Count; ++i)
            {
                if ((i + 1) % 7 == 0 && i > 0)
                {
                    indexLastday = i + 1;
                    resultateAVGMarks.Add(new Pair<string, double>($"Week from {datesMarks[indexLastday-7].First} to {datesMarks[i].First}: ", 
                                          CalculatingAVGMark(datesMarks.GetRange(i - 7, 7))));
                }
                else if ((i + 1) == datesMarks.Count)
                {
                    resultateAVGMarks.Add(new Pair<string, double>($"Week from {datesMarks[i-(i-indexLastday)].First} to {datesMarks[i].First}: ",
                                          CalculatingAVGMark(datesMarks.GetRange(i - 7, 7))));
                }
            }
            return resultateAVGMarks;
        }

        private List<Pair<string, double>> CalculatingAVGMarkMonthly(List<Pair<DateTime, int>> datesMarks)
        {
            List<Pair<string, double>> resultateAVGMarks = new List<Pair<string, double>>();
            DateTime monthDayFirst =new DateTime(datesMarks[0].First.Year, datesMarks[0].First.Month, 1);
            int indexFirstDay = 0;
            DateTime monthDayLast = (datesMarks[0].First.AddMonths(1).AddDays(-1) < datesMarks[datesMarks.Count - 1].First) ? datesMarks[0].First.AddMonths(1).AddDays(-1) : datesMarks[datesMarks.Count - 1].First; ;

            if (datesMarks.Count == 0)
            {
                return new List<Pair<string, double>>();
            }
                        
            for (int i = 0; i < datesMarks.Count; ++i)
            {
                if (monthDayLast < datesMarks[i].First)
                {
                    resultateAVGMarks.Add(new Pair<string, double>($"Month - {datesMarks[i].First.Month}: ", CalculatingAVGMark(datesMarks.GetRange(i - monthDayLast.Day, monthDayLast.Day))));
                    indexFirstDay = i;
                    monthDayLast = datesMarks[i].First.AddMonths(1).AddDays(-1);
                }
                else if ((i + 1) == datesMarks.Count)
                {
                    resultateAVGMarks.Add(new Pair<string, double>($"Month - {datesMarks[i].First.Month}:  ", CalculatingAVGMark(datesMarks.GetRange(i - (i - indexFirstDay), i - indexFirstDay))));
                }
            }
            return resultateAVGMarks;
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
