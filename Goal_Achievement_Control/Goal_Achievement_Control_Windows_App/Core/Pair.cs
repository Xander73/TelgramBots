using System;
using System.Collections.Generic;
using System.Text;

namespace Goal_Achievement_Control_Windows_App.Core
{
    class Pair<T, V>
    {
        public Pair(T first, V second)
        {
            First = first;
            Second = second;
        }
        public Pair() { }
        public T First { get; set; }
        public V Second { get; set; }
    }
}

