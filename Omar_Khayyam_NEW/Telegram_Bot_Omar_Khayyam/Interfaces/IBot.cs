using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Telegram_Bot_Omar_Khayyam.Interfaces
{
    interface IBot
    {
        delegate void BotHandlerDel(object obj, DoWorkEventArgs e);

        void bw_DoWork(object sender, DoWorkEventArgs e);


    }
}
