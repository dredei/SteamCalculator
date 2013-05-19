using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SteamCalculator
{
    public partial class frmSteamGuard : Form
    {
        frmMain Host;

        public frmSteamGuard(frmMain Host)
        {
            InitializeComponent();
            this.Host = Host;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Host.guardCode = textBox1.Text;
            this.Close();
        }
    }
}
