using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Telegram_Bot_Omar_Khayyam.Interfaces
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
