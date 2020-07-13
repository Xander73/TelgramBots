using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw;
        string[] Poems;

        public Form1()
        {
            InitializeComponent();

            notifyIcon1.Visible = false;
           
            initilizePoems();

            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
                        
            this.bw.RunWorkerAsync("859571517:AAFUDLZtmPVJK_xyhbP2Reqigr_xo0Lgh5M");    //token
            Connect.Text = "Bot is runing";     //text on the button
        }
                
        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String;
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key);
                await Bot.SetWebhookAsync("");
                int offset = 0;

                var ud = await Bot.GetUpdatesAsync(offset);

                foreach (var v in ud)
                {
                    //var mes = v.Message;
                    //var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Раз"));
                    //await Bot.SendTextMessageAsync(mes.Chat.Id, "Reply кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                }

                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset);
                    foreach (var v in updates)
                    {
                        var message = v.Message;
                        if (message == null) return;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                            if (message.Text == "/saysomething")    //answer
                            {
                                // в ответ на команду /saysomething выводим сообщение
                                await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                            }
                            if (message.Text == "/ibuttons")    //inline buttons
                            {
                                var keyBoard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                                    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
                                    {
                                        new []
                                        {
                                            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData ("Встряхнуть", "callBack1"),

                                            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton.WithCallbackData("Положить", "callBack2"),
                                        },
                                    }
                                    );
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Инлайн кнопка работает", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                            }
                            if (message.Text == "/rbuttons" || message.Text == "/to_shake" || message.Text == "Встряхнуть!")
                            {
                                var keyBoard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Встряхнуть!"));
                                await Bot.SendTextMessageAsync(message.Chat.Id, processingAndReturnReply(), Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyBoard);
                            }
                        }

                        //callBacks

                        offset = v.Id + 1;
                    }

                }
            }

            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        
        private void initilizePoems()
        {
            Poems = new string[1306];    //1306 poems in file
            string[] poemsTemp = WindowsFormsApp1.Properties.Resources.Poems.Split('\n');
            for (int i = 0; i < poemsTemp.Length; ++i)
            {
                if (int.TryParse(poemsTemp[i], out int tempIndex))  //if digit, the next line for us
                {
                    while (i < poemsTemp.Length - 1 && !int.TryParse(poemsTemp[i + 1], out int o))
                    {
                        Poems[tempIndex - 1] += poemsTemp[++i] + '\n';  //add the lines of one poem
                    }
                }
            }
        }

        private string processingAndReturnReply() 
        {
            Random rn = new Random();
            return Poems[rn.Next(0, 1305)];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
