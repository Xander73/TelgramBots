using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1();

            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                icon.ContextMenu = new ContextMenu(
                    new []
                    {
                new MenuItem("Show form", (s, e) => form.Show()),
                new MenuItem("Hide form", (s, e) => form.Hide()),
                new MenuItem("Exit", (s, e) => Application.Exit()),
                    });
                icon.Visible = true;

                Application.Run();
                icon.Visible = false;
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
