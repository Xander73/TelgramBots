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
            
        }        

        public string Name { get; set; }   //name of Goal

        private List<Dictionary<DateTime, int>> marks = new List<Dictionary<DateTime, int>>();    //goals achievement assessment

        public List<Dictionary<DateTime, int>> Marks
        {
            get => marks;

            set
            {
                marks.AddRange (new List<Dictionary<DateTime, int>> (value));
            }           
        }

    }
}
