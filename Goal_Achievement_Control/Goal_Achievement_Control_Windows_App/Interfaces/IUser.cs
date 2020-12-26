using System;
using System.Collections.Generic;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Interfaces
{
    interface IUser
    {
        string GoalsToString();

        string AddGoal(string goal);

        string DeleteGoal(int goalIndex);

        int CountGoals();

        string AddMarks(string text);
    }
}
