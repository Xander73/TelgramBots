using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsFormsApp1.Core;

namespace WindowsFormsApp1.Bot
{
    class Token : IToken
    {
        string _key;
        public Token (string k)
        {
            _key = k;
        }

        public string Key { get => _key; }

        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ;
        }
    }
}
