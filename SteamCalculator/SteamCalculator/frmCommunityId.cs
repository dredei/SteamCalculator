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
    public partial class frmCommunityId : Form
    {
        frmMain Host;

        public frmCommunityId(frmMain Host)
        {
            InitializeComponent();
            this.Host = Host;
        }

        private void btnOk_Click( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty( tbSteamId.Text ) )
            {
                MessageBox.Show( "Enter SteamId", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            Host.communityId = tbSteamId.Text;
            this.Close();
        }
    }
}
