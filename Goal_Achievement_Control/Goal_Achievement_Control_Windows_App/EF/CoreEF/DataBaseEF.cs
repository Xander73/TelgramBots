using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Goal_Achievement_Control_Windows_App.Interfaces;
using System.Globalization;
using System.Data;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using Goal_Achievement_Control_Windows_App.EF.Context;
using Goal_Achievement_Control_Windows_App.EF.Models;
using System.Linq;

namespace Goal_Achievement_Control_Windows_App.EF.CoreEF
{
    public class DataBaseEF : IDataBase
    {
        private string nameDataBase;
        private myDBContext dbContext;
        public string NameDataBase { get => nameDataBase; }

        public DataBaseEF(myDBContext dbContext)
        {
            this.dbContext = dbContext;
            //!!!!!!!!!!!!! метод от балды. Надо задать имя базы данных
            this.nameDataBase =  dbContext.Database.ProviderName;            
        }

        public void AddUser(string telegramId, string chatId, OperatingMode mode)
        {
            dbContext.Users.Add(new User { TelegramId = telegramId, ChatId = chatId, OperatingMode = mode.ToString() });
            dbContext.SaveChanges();
        }

        public void AddGoal(string goal, int userId)
        {
            dbContext.Goals.Add(new Goal { Goal1 = goal, UserId = userId });
            dbContext.SaveChanges();
        }

        public void AddMarks(int userId, string[] marks, List<int> goalsId)
        {
            for (int i = 0; i < goalsId.Count; ++i)
            {
                dbContext.Marks.Add(new Mark { Date = DateTime.Now.ToShortDateString(), Mark1 = marks[i], GoalId = goalsId[i] });
            }
            dbContext.SaveChanges();
        }

        public OperatingMode ChangeOperatingMode (int idUser, OperatingMode om)
        {
            (from user in dbContext.Users where user.Id == idUser select user).Single().OperatingMode = om.ToString();
            dbContext.SaveChanges();
            
            return om;
        }               

        public OperatingMode GetUserMod(int id)
        {
            switch ((from user in dbContext.Users where user.Id == id select user.OperatingMode).Single())
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

        //!!!!!!!!!!!!!!!! убрать этот метод
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

        public int IdCurrentUser(int telegramId) => 
            (from user in dbContext.Users where int.Parse(user.TelegramId) == telegramId select (int)user.Id).SingleOrDefault();            
        
        public void AddTable(string nameTable, string columnsWithAttributes) { }
        
        public Dictionary<string, string> GetTelegramIdUsers ()
        {
            Dictionary<string, string> telegramIdUsers = new Dictionary<string, string>();
            var users = from user in dbContext.Users select new { user.TelegramId, user.Id };
            foreach (var item in users)
            {
                telegramIdUsers.Add(item.TelegramId, item.Id.ToString());
            }
            return telegramIdUsers;
        }

        public Dictionary<int, string> GetGoals(int userId)
        {
            Dictionary<int, string> resultate = new Dictionary<int, string>();
            var goals = from goal in dbContext.Goals where goal.UserId == userId select new { goal.Id, goal.Goal1 };
            foreach (var item in goals)
            {
                resultate.Add((int)item.Id, item.Goal1);
            }
            return resultate;
        }

        public string DeleteGoal(int userId, int goalIndex)
        {
            int indexForList = goalIndex - 1;
            List<string> goals = new List<string>(GetGoals(userId).Values);
            dbContext.Goals.Remove((from goal in dbContext.Goals where goal.UserId == userId && goal.Goal1 == goals[indexForList] select goal).Single());
            dbContext.SaveChanges();
            
            return goals[indexForList] + " удалена";
        }

        public int CountGoals(int id) => (from goal in dbContext.Goals select goal).Count();
            

        public string MarksLastFourWeeks(int userId)    //ID в базе данных приложения
        {
            Dictionary<int, string> goalsCurentUser = GetGoals(userId);
            List<KeyValuePair<DateTime, int>> dateMarks = new List<KeyValuePair<DateTime, int>>();
            List<KeyValuePair<string, double>> AVGMarks = new List<KeyValuePair<string, double>>();
            string resultate = null;

            List<int> idGoals = new List<int>(goalsCurentUser.Keys);
            for (int i = 0; i < goalsCurentUser.Count - 1; i++)
            {
                var items = (from user in dbContext.Users
                             join goal in dbContext.Goals on user.Id equals goal.UserId
                             join mark in dbContext.Marks on goal.Id equals mark.GoalId
                             where goal.Id == idGoals[i]
                             orderby goal.Id descending
                             select new { user.TelegramId, goal.Goal1, mark.Date, mark.Mark1 }).Take(28); //28 дней = 4 недели
            }

            using (var connection = new SQLiteConnection($"Data Source = {nameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
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
            List<KeyValuePair<DateTime, int>> dateMarksWeek = new List<KeyValuePair<DateTime, int>>();
            List<KeyValuePair<string, double>> AVGMarksWeeks = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<DateTime, int>> dateMarksAll = new List<KeyValuePair<DateTime, int>>();
            List<KeyValuePair<string, double>> AVGMarksMonths = new List<KeyValuePair<string, double>>();
            string resultate = null;

            List<int> idGoals = new List<int>(goalsCurentUser.Keys);

            for (int i = 0; i < goalsCurentUser.Count - 1; i++)
            {
                string tempResultate = goalsCurentUser[i].ToString() + "\n\n";

                var marks = from user in dbContext.Users
                            join goal in dbContext.Goals on user.Id equals goal.UserId
                            join mark in dbContext.Marks on goal.Id equals mark.GoalId
                            where goal.Id == idGoals[i]
                            orderby goal.Id descending
                            select new { user.TelegramId, goal.Goal1, mark.Date, mark.Mark1 };
            }

            using (var connection = new SQLiteConnection($"Data Source = {NameDataBase}"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
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
