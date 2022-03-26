using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mudo
{
    public partial class ControlForm : Form
    {

        private const int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        protected override void WndProc(ref Message m) //Supress mouse focus
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)MA_NOACTIVATE;
                return;
            }
            base.WndProc(ref m);
        }

        Mudo parent;
        protected override bool ShowWithoutActivation => true;
        public ControlForm(Mudoz parent)
        {
            this.ControlBox = false;
            InitializeComponent();
            this.parent = parent;

        }

        public bool supressmove = true;

        private void onMove(object sender, EventArgs e)
        {
            if (Visible)
            {
                if(!supressmove) 
                    parent.Window.Position = new Microsoft.Xna.Framework.Point(Location.X - parent.Window.ClientBounds.Width, Location.Y);
            }
        }

    }
}
