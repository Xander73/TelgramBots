using Goal_Achievement_Control_Windows_App.Core;
using Goal_Achievement_Control_Windows_App.CurrentBot;
using System.Windows.Forms;

namespace Goal_Achievement_Control_Windows_App
{
    public partial class Goal_Achievement_Control : Form
    {
        public Goal_Achievement_Control()
        {
            InitializeComponent();

            BaseBot bot = new MainBot();        //композиция

            
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            
        }

    }
}
