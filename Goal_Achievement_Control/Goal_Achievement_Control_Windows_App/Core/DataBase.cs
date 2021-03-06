﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Goal_Achievement_Control_Windows_App.Interfaces;
using System.Globalization;
using System.Data;
using Goal_Achievement_Control_Windows_App.CurrentBot;

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
            }

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

        public OperatingMode ChangeOperatingMode (int idUser, OperatingMode om)
        {
            using (var connected = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE Users SET operatingMode = '{om.ToString()}' WHERE id == {idUser}";
                    cmd.ExecuteNonQuery();
                }
                return om;
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
                            case "AddMark":
                                return OperatingMode.AddMark;
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
                        while (reader.Read())
                        {
                            resultate.Add(Convert.ToInt32(reader["id"]), reader["Goal"].ToString());
                        }
                        return resultate;
                    }
                }
            }
        }

        public string DeleteGoal(int userId, int goalIndex)
        {
            int indexForList = goalIndex - 1;
            List<string> goals = new List<string>(GetGoals(userId).Values);

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM Goals WHERE userId == {userId} AND Goal == '{goals[indexForList]}'";
                    cmd.ExecuteNonQuery();
                    return goals[indexForList] + " удалена";
                }
            }
        }

        public int CountGoals(int id)
        {
            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT Count (Goal) FROM Goals WHERE {id} == userId";
                    cmd.CommandType = CommandType.Text;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public string MarksLastFourWeeks(int userId)    //ID в базе данных приложения
        {
            Dictionary<int, string> goalsCurentUser = GetGoals(userId);
            List<KeyValuePair<DateTime, int>> dateMarks = new List<KeyValuePair<DateTime, int>>();
            List<KeyValuePair<string, double>> AVGMarks = new List<KeyValuePair<string, double>>();
            string resultate = null;

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    List<int> idGoals = new List<int>(goalsCurentUser.Keys);
                    for (int i = 0; i < idGoals.Count; i++)
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
                                dateMarks.Add(new KeyValuePair<DateTime, int>(Convert.ToDateTime(reader["Date"]), Convert.ToInt32(reader["mark"])));
                                if ((dateMarks.Count % 7 == 0 && dateMarks.Count != 0) /*|| dateMarks[dateMarks.Count - 1].First.DayOfWeek == DayOfWeek.Monday*/)
                                {                                    
                                    AVGMarks.Add(new KeyValuePair<string, double>($"Average weekly score:\nfrom {dateMarks[0].Key.ToShortDateString()} to {dateMarks[dateMarks.Count - 1].Key.ToShortDateString()}", CalculatingAVGMark(dateMarks)));
                                    dateMarks.Clear();
                                }
                                tempResultate += $"{reader["Date"]} - {reader["mark"]}\n";
                            }
                        }
                        resultate += tempResultate + "______________________________________________\n\n";

                        if (AVGMarks.Count == 0)
                        {
                            return "Average weekly score:\nВы недавно начали движение к цели.\nОценок нет.";
                        }
                        else
                        {
                            foreach (var v in AVGMarks)
                            {
                                resultate += $"{v.Key} - {v.Value};\n";
                            }
                        }                                                
                    }
                    return resultate == null ? "Average weekly score:\nВы недавно начали движение к цели.\nОценок нет." : resultate;
                }
            }
        }

        public string MarksAll(int userId)
        {
            Dictionary<int, string> goalsCurentUser = GetGoals(userId);
            List<KeyValuePair<DateTime, int>> dateMarksAll = new List<KeyValuePair<DateTime, int>>();
            string resultate = null;

            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    List<int> idGoals = new List<int>(goalsCurentUser.Keys);
                    for (int i = 0; i < goalsCurentUser.Count; i++)
                    {
                        string tempResultate = goalsCurentUser[idGoals[i]] + "\n\n";
                        cmd.CommandText = $"SELECT telegramId, Goal, Date, mark FROM Users " +
                            $"JOIN Goals ON Users.id == Goals.userId " +
                            $"JOIN Marks ON Goals.id == Marks.goal_id " +
                            $"WHERE Goals.id == '{idGoals[i]}' "; //+
                           // $"ORDER BY Goals.id DESC";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                dateMarksAll.Add(new KeyValuePair<DateTime, int>(Convert.ToDateTime(reader["Date"]), (Convert.ToInt32(reader["mark"]))));
                            }
                        }

                        if (dateMarksAll.Count == 0)
                            return "оценок нет.";
                        string tempAVGMarkWeekly = "";
                        foreach (var v in CalculatingAVGMarkWeekly(dateMarksAll))
                        {
                            tempAVGMarkWeekly += v.Key + v.Value.ToString() + '\n';
                        }

                        string tempAVGMarkMonthly = "";
                        foreach (var v in CalculatingAVGMarkMonthly(dateMarksAll))
                        {
                            tempAVGMarkMonthly += v.Key + v.Value.ToString() + '\n';
                        }
                        dateMarksAll.Clear();

                        resultate += tempResultate + tempAVGMarkWeekly +
                            "______________________________________________\n\n" +
                            tempAVGMarkMonthly + '\n';
                    }                        
                    return resultate;
                }
            }
        }


        #region support_functions

        private double CalculatingAVGMark(List<KeyValuePair<DateTime, int>> datesMarks)
        {
            double markAVG = 0;
            foreach (var dateMark in datesMarks)
            {
                markAVG += dateMark.Value;
            }
            return markAVG /= datesMarks.Count;
        }

        private List<KeyValuePair<string, double>> CalculatingAVGMarkWeekly(List<KeyValuePair<DateTime, int>> datesMarks)
        {
            List<KeyValuePair<string, double>> resultateAVGMarks = new List<KeyValuePair<string, double>>();
            int indexLastCheckedDay = 0;
            const int DAYS_IN_WEEK = 7;

            if (datesMarks.Count == 0)
            {
                return new List<KeyValuePair<string, double>>();
            }

            for (int i = 0; i < datesMarks.Count; ++i)
            {
                if ((i + 1) % DAYS_IN_WEEK == 0 && i > 0)
                {
                    indexLastCheckedDay = i + 1;
                    resultateAVGMarks.Add(new KeyValuePair<string, double>($"Week from {datesMarks[indexLastCheckedDay- DAYS_IN_WEEK].Key.ToShortDateString()} to {datesMarks[i].Key.ToShortDateString()}: ", 
                                          CalculatingAVGMark(datesMarks.GetRange(indexLastCheckedDay - DAYS_IN_WEEK, DAYS_IN_WEEK))));
                }
                else if ((i + 1) == datesMarks.Count)
                {
                    if (datesMarks.Count < DAYS_IN_WEEK)
                    {
                        resultateAVGMarks.Add(new KeyValuePair<string, double>($"Week from {datesMarks[i - (i - indexLastCheckedDay)].Key.ToShortDateString()} to {datesMarks[i].Key.ToShortDateString()}: ",
                                          CalculatingAVGMark(datesMarks.GetRange(i - (i - indexLastCheckedDay), datesMarks.Count))));
                    }
                    else
                    {
                        resultateAVGMarks.Add(new KeyValuePair<string, double>($"Week from {datesMarks[i - (i - indexLastCheckedDay)].Key.ToShortDateString()} to {datesMarks[i].Key.ToShortDateString()}: ",
                                          CalculatingAVGMark(datesMarks.GetRange(i - (i - indexLastCheckedDay), i - indexLastCheckedDay))));
                    }                    
                }
            }
            return resultateAVGMarks;
        }

        private List<KeyValuePair<string, double>> CalculatingAVGMarkMonthly(List<KeyValuePair<DateTime, int>> datesMarks)
        {
            if (datesMarks.Count == 0)
                return null;

            List<KeyValuePair<string, double>> resultateAVGMarks = new List<KeyValuePair<string, double>>();
            int indexFirstDay = 0;
            DateTime monthDayLast = (datesMarks[0].Key.AddMonths(1).AddDays(-1) < datesMarks[datesMarks.Count - 1].Key) ? datesMarks[0].Key.AddMonths(1).AddDays(-1) : datesMarks[datesMarks.Count - 1].Key; ;

            if (monthDayLast == null)
                return null;

            if (datesMarks.Count == 0)
            {
                return new List<KeyValuePair<string, double>>();
            }
                        
            for (int i = 0; i < datesMarks.Count; ++i)
            {
                if (monthDayLast < datesMarks[i].Key)
                {
                    resultateAVGMarks.Add(new KeyValuePair<string, double>($"Month - {datesMarks[i].Key.ToString("MMM", CultureInfo.CurrentCulture)}: ", CalculatingAVGMark(datesMarks.GetRange(i - monthDayLast.Day, monthDayLast.Day))));
                    indexFirstDay = i;
                    monthDayLast = datesMarks[i].Key.AddMonths(1).AddDays(-1);
                }
                else if ((i + 1) == datesMarks.Count)
                {
                    resultateAVGMarks.Add(new KeyValuePair<string, double>($"Month - {datesMarks[i].Key.ToString("MMMM", CultureInfo.CurrentCulture)}:  ", CalculatingAVGMark(datesMarks.GetRange(i - (i - indexFirstDay), i - indexFirstDay+1))));
                }
            }
            return resultateAVGMarks;
        }

        public void ClearTable(string nameTable)
        {
            using (var connacted = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connacted.Open();
                using (var cmd = connacted.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {nameTable}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ClearAllTables ()
        {
            using (var connected = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connected.Open();
                using (var cmd = connected.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Users";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM Goals";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM Marks";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion               
    }
}
