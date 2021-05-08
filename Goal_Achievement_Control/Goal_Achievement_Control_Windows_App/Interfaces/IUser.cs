using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        //E:\0_work\C#\Telegram_Bot\Telegram_Bots\Goal_Achievement_Control\Goal_Achievement_Control_Windows_App\bin\Debug\netcoreapp3.1\myDB.db
    }
}
