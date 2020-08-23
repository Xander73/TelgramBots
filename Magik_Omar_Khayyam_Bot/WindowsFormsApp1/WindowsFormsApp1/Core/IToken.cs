using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WindowsFormsApp1.Core
{
    interface IToken
    {        
        string Key { get; }        

        /*async*/ void bw_DoWork(object sender, DoWorkEventArgs e);
    }
}
