using System;
using System.Collections.Generic;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Interfaces
{
    public interface IUser
    {
        Telegram.Bot.Types.Message Message { get; set; }
        IDataBase DataBase { get; }
        string GoalsToString();

        string AddGoal(string goal);

        string DeleteGoal(int goalIndex);

        int CountGoals();

        string AddMarks(string text);
    }
}
