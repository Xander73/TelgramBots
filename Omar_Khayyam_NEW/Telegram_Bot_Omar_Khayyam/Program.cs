using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telegram_Bot_Omar_Khayyam
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new WindowOfToken();
            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                icon.ContextMenuStrip = new ContextMenuStrip();
                icon.ContextMenuStrip.Items.Add("Show form", Image.FromFile("icon.png"), (s, e) => form.Show());
                icon.ContextMenuStrip.Items.Add("Hide form", Image.FromFile("icon.png"), (s, e) => form.Hide());
                icon.ContextMenuStrip.Items.Add("Exit", Image.FromFile("icon.png"), (s, e) => Application.Exit());
                icon.Visible = true;

                Application.Run();
                icon.Visible = false;
            }
        }
    }
}
