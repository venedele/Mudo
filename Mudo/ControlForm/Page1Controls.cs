using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mudo
{
    public partial class ControlForm : Form
    {

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.defeat1.Text = parent.defeat1.ToString();
            this.defeat2.Text = parent.defeat2.ToString();
        }
    }
}
