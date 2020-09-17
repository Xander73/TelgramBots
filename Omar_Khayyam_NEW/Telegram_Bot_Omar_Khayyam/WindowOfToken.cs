using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Telegram_Bot_Omar_Khayyam.Interfaces;
using Telegram_Bot_Omar_Khayyam.Bot;

namespace Telegram_Bot_Omar_Khayyam
{
    public partial class WindowOfToken : Form
    {
        public WindowOfToken()
        {
            InitializeComponent();

            BaseBot bot = new BotOmarKhayyam();        //композиция

            //context menu
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.ContextMenuStrip.Items.Add ("Show form", Image.FromFile("icon.png"), (s, e) => this.Show());
            this.ContextMenuStrip.Items.Add("Hide form", Image.FromFile("icon.png"), (s, e) => this.Hide());
            this.ContextMenuStrip.Items.Add("Exit", Image.FromFile("icon.png"), (s, e) => Application.Exit());
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
