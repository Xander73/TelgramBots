using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot_Omar_Khayyam.Interfaces
{
    interface IBotSettings
    {
        void botsAnswerAsync(Telegram.Bot.Types.Message message);

        void BotsWork();
    }
}
