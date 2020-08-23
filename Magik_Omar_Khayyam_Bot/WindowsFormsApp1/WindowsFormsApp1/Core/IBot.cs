using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Core
{
    interface IBot
    {
        Task<Telegram.Bot.Types.Message> botsAnswerAsync (Telegram.Bot.Types.Message message);

        void botsWork();
    }
}
