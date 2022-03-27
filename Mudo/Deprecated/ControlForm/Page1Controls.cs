using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Mudo
{
    public partial class ControlForm : Form
    {
        private void setPauseText()
        {
            pause.Text = parent.Pause?"Start":"Stop";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.defeat1.Text = parent.defeat1.ToString();
            this.defeat2.Text = parent.defeat2.ToString();
            setPauseText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.TogglePause();
            setPauseText();
            //TODO: Cannot switch back to main window
        }

    }
}
