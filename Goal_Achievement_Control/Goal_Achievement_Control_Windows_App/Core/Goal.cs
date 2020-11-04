/*
 * Класс цели, которую хочет достигнуть пользователь.
 * Хранит имя и оценку в виде (дата, оценка).
 * */

using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Goal_Achievement_Control.Helpers
{
    class Goal
    {
        public Goal()
        {
            mark = new Dictionary<DateTime, short>();
        }

        public Goal (string goal) : this ()
        {
            Name = goal;
        }

        public string Name { get; set; }   //name of Goal

        private Dictionary<DateTime, short> mark = new Dictionary<DateTime, short>();    //goals achievement assessment

        void AddNewMark (short mark)
        {
            bool isWrongValue = true;
            while (isWrongValue)
            if (mark > 0 && mark <=10)
            {
                this.mark.Add(DateTime.Now, mark);
                isWrongValue = false;
            }
            else
            {
                Console.WriteLine("Wrong value.\nPlease, repeat.");
            }
        }
    }
}
