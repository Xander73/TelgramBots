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
            evaluations = new Dictionary<DateTime, short>();
        }

        public string Name { get; set; }   //name of Goal

        private Dictionary<DateTime, short> evaluations = new Dictionary<DateTime, short>();    //goals achievement assessment

        void AddNewEvaluation (short evaluation)
        {
            bool isWrongValue = true;
            while (isWrongValue)
            if (evaluation > 0 && evaluation <=10)
            {
                evaluations.Add(DateTime.Now, evaluation);
                isWrongValue = false;
            }
            else
            {
                Console.WriteLine("Wrong value.\nPlease, repeat.");
            }
        }
    }
}
