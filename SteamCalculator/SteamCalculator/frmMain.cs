using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SteamCalculator
{
    public partial class frmMain : Form
    {
        public string guardCode = string.Empty;
        SteamCalculator steamCalc = null;
        List<SteamCalculator.games> games = null;
        double sumPrice = 0;
        Thread thr = null;
        public string steamId; //76561198013216436

        public frmMain()
        {
            InitializeComponent();
        }

        private void calculatePrice()
        {
            try
            {
                games = steamCalc.getGames( steamId );
                MessageBox.Show( "Sum: $" + sumPrice.ToString(), "Sum", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            catch ( Exception e )
            {
                MessageBox.Show( e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                btnStart.Enabled = true;
            }
        }

        private void centerLables( Label lbl )
        {
            int l = this.Width / 2 - lbl.Width / 2;
            lbl.Location = new Point( l, lbl.Location.Y );
        }

        private void button1_Click( object sender, EventArgs e )
        {
            btnStart.Enabled = false;
            lvGames.Items.Clear();
            sumPrice = 0;
            frmSteamId frm = new frmSteamId( this );
            frm.ShowDialog();
            List<SteamCalculator.games> gamesList = new List<SteamCalculator.games>();
            thr = new Thread( calculatePrice );
            thr.Start();
        }

        private void frmMain_Load( object sender, EventArgs e )
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            steamCalc = new SteamCalculator();
            steamCalc.updEvent += new EventHandler<UpdEventArgs>( updProgress );
            centerLables( lblInfo );
            centerLables( lblSum );
        }

        private void updProgress( object sender, UpdEventArgs e )
        {
            pb1.Maximum = e.maxProgress;
            pb1.Value = e.progress;
            string str = e.msg;
            if ( !String.IsNullOrEmpty( e.msg ) )
            {
                string name = str.Substring( 0, str.IndexOf( ';' ) );
                string price = str.Substring( str.IndexOf( ';' ) + 1, str.Length - str.IndexOf( ';' ) - 1 );
                ListViewItem lvi = new ListViewItem();
                lvi.Text = name;

                lvi.SubItems.Add( "$" + price );
                lvGames.Items.Add( lvi );
                lvGames.Items[ lvGames.Items.Count - 1 ].EnsureVisible();
                sumPrice += double.Parse( price );
            }
            lblInfo.Text = "{0} / {1}".f( e.progress, e.maxProgress );
            lblSum.Text = "Sum: ${0}".f( sumPrice );
            centerLables( lblInfo );
            centerLables( lblSum );
        }

        private void frmMain_Resize( object sender, EventArgs e )
        {
            centerLables( lblInfo );
            centerLables( lblSum );
            lvGames.Columns[ 0 ].Width = (int)( ( this.Width - 43 ) * 0.73 );
            lvGames.Columns[ 1 ].Width = (int)( ( this.Width - 43 ) * 0.27 );
        }

        private void frmMain_FormClosed( object sender, FormClosedEventArgs e )
        {
            thr.Abort();
        }
    }
}
