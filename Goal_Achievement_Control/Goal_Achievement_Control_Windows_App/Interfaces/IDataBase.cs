using System;
using System.Collections.Generic;
using System.Text;
using Goal_Achievement_Control.CurrentBot;
using Goal_Achievement_Control_Windows_App.Core;

namespace Goal_Achievement_Control_Windows_App.Interfaces
{
    interface IDataBase
    {
        void AddData(string nameTable, string data);

        void AddUser(string telegramId, string cahtId, OperatingMode mode);

        void AddGoal(string goal, int userId);

        public void AddMarks(int userId, string[] marks, List<int> goalsId);

        int NextId(string nameTable);

        int IdCurrentUser(int telegramId);              

        void ChangeOperatingMode(int userId, OperatingMode mode);

        Dictionary<string, string> TelegramIdUsers();

        Dictionary<int, string> GetGoals(int userId);

        string DeleteGoal(int userId, int goalIndex);

        string MarksLastFourWeeks(int userId);

        string MarksAll(int userId);
    }
}
