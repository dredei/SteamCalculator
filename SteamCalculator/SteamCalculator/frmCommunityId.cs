#region Using

using System;
using System.Windows.Forms;

#endregion

namespace SteamCalculator
{
    public partial class FrmCommunityId : Form
    {
        private readonly FrmMain _host;

        public FrmCommunityId( FrmMain host )
        {
            InitializeComponent();
            this._host = host;
        }

        private void btnOk_Click( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty( tbSteamId.Text ) )
            {
                MessageBox.Show( "Enter SteamId", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            this._host.CommunityId = tbSteamId.Text;
            this.Close();
        }
    }
}