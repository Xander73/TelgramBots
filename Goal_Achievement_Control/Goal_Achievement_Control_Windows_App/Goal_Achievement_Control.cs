using Goal_Achievement_Control.BaseBot;
using Goal_Achievement_Control.CurrentBot;

using System.Windows.Forms;

namespace Goal_Achievement_Control_Windows_App
{
    public partial class Goal_Achievement_Control : Form
    {
        public Goal_Achievement_Control()
        {
            InitializeComponent();

            BaseBot bot = new MainBot();        //композиция

            //context menu
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            //this.ContextMenuStrip.Items.Add("Show form", Image.FromFile("icon.png"), (s, e) => this.Show());
            //this.ContextMenuStrip.Items.Add("Hide form", Image.FromFile("icon.png"), (s, e) => this.Hide());
            //this.ContextMenuStrip.Items.Add("Exit", Image.FromFile("icon.png"), (s, e) => Application.Exit());
        }

    }
}
