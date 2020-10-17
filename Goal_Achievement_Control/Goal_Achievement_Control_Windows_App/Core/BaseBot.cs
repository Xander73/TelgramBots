using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Goal_Achievement_Control.BaseBot
{
    abstract class BaseBot
    {
        //delegate void BotHandlerDel(object obj, DoWorkEventArgs e);

        BackgroundWorker bw;

        public BaseBot()
        {
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync("859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M");    //token


        }

        public abstract void bw_DoWork(object sender, DoWorkEventArgs e);


    }
}
