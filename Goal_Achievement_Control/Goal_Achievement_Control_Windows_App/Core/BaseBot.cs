using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Security.Principal;

namespace Goal_Achievement_Control.BaseBot
{
    abstract class BaseBot
    {
        //delegate void BotHandlerDel(object obj, DoWorkEventArgs e);

        private BackgroundWorker bw;
        protected const string token = "859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M";
        public BaseBot()
        {
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync(token);    //token
        }

        public abstract void bw_DoWork(object sender, DoWorkEventArgs e);


    }
}
