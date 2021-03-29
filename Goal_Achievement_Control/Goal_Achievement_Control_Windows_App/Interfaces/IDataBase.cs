using Goal_Achievement_Control_Windows_App.CurrentBot;
using System.Collections.Generic;

namespace Goal_Achievement_Control_Windows_App.Interfaces
{
    public interface IDataBase
    {
        string NameDataBase { get; }

        void AddData(string nameTable, string data);

        void AddUser(string telegramId, string cahtId, OperatingMode mode);

        void AddGoal(string goal, int userId);

        public void AddMarks(int userId, string[] marks, List<int> goalsId);

        int NextId(string nameTable);

        int IdCurrentUser(int telegramId);              

        void ChangeOperatingMode(int userId, OperatingMode mode);

        Dictionary<string, string> GetTelegramIdUsers();

        Dictionary<int, string> GetGoals(int userId);

        string DeleteGoal(int userId, int goalIndex);

        string MarksLastFourWeeks(int userId);

        string MarksAll(int userId);

        OperatingMode GetUserMod(int id);

        int CountGoals(int userId);

        OperatingMode AddOperatingMode(int idUser, OperatingMode om);
    }
}
