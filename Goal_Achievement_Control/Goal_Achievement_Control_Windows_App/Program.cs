using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Goal_Achievement_Control_Windows_App
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Goal_Achievement_Control());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Goal_Achievement_Control();
            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                icon.ContextMenuStrip = new ContextMenuStrip();
               // icon.ContextMenuStrip.Items.Add("Show form", Image.FromFile("icon.png"), (s, e) => form.Show());
                //icon.ContextMenuStrip.Items.Add("Hide form", Image.FromFile("icon.png"), (s, e) => form.Hide());
                //icon.ContextMenuStrip.Items.Add("Exit", Image.FromFile("icon.png"), (s, e) => Application.Exit());
                icon.Visible = true;

                Application.Run();
                icon.Visible = false;
            }
        }
    }
}
