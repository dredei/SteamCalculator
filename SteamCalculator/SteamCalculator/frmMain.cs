#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace SteamCalculator
{
    public partial class FrmMain : Form
    {
        private SteamCalculator _steamCalc;
        private List<SteamCalculator.Games> _games = null;
        private double _sumPrice;
        private Thread _thr = null;
        public string CommunityId;
        private readonly Stopwatch _stopWatch;

        public FrmMain()
        {
            InitializeComponent();
            this._stopWatch = new Stopwatch();
        }

        private void CalculatePrice()
        {
            this._stopWatch.Start();
            try
            {
                this._games = this._steamCalc.GetGames( this.CommunityId );
                MessageBox.Show( "Sum: $" + this._sumPrice + "\nTime: " + this._stopWatch.Elapsed, "Sum",
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            catch ( Exception e )
            {
                MessageBox.Show( e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                this._stopWatch.Stop();
                btnStart.Enabled = true;
            }
        }

        private void CenterLables( Label lbl )
        {
            int l = this.Width / 2 - lbl.Width / 2;
            lbl.Location = new Point( l, lbl.Location.Y );
        }

        private void UpdProgress( object sender, UpdEventArgs e )
        {
            pb1.Maximum = e.MaxProgress;
            pb1.Value = e.Progress;
            string str = e.Msg;
            if ( !String.IsNullOrEmpty( e.Msg ) )
            {
                string name = str.Substring( 0, str.IndexOf( ';' ) );
                string price = str.Substring( str.IndexOf( ';' ) + 1, str.Length - str.IndexOf( ';' ) - 1 );
                var lvi = new ListViewItem();
                lvi.Text = name;

                lvi.SubItems.Add( "$" + price );
                lvGames.Items.Add( lvi );
                lvGames.Items[ lvGames.Items.Count - 1 ].EnsureVisible();
                this._sumPrice += double.Parse( price );
            }
            lblInfo.Text = "{0} / {1}".F( e.Progress, e.MaxProgress );
            lblSum.Text = "Sum: ${0}".F( this._sumPrice );
            this.CenterLables( lblInfo );
            this.CenterLables( lblSum );
        }

        private void frmMain_Load( object sender, EventArgs e )
        {
            CheckForIllegalCrossThreadCalls = false;
            this._steamCalc = new SteamCalculator();
            this._steamCalc.updEvent += new EventHandler<UpdEventArgs>( this.UpdProgress );
            this.CenterLables( lblInfo );
            this.CenterLables( lblSum );
        }

        private void frmMain_Resize( object sender, EventArgs e )
        {
            this.CenterLables( lblInfo );
            this.CenterLables( lblSum );
            lvGames.Columns[ 0 ].Width = (int)( ( this.Width - 43 ) * 0.73 );
            lvGames.Columns[ 1 ].Width = (int)( ( this.Width - 43 ) * 0.27 );
        }

        private void frmMain_FormClosed( object sender, FormClosedEventArgs e )
        {
            if ( this._thr != null )
            {
                this._thr.Abort();
            }
        }

        private void btnStart_Click( object sender, EventArgs e )
        {
            btnStart.Enabled = false;
            lvGames.Items.Clear();
            this._sumPrice = 0;
            var frm = new FrmCommunityId( this );
            DialogResult dlgRes = frm.ShowDialog();
            if ( dlgRes != DialogResult.OK )
            {
                btnStart.Enabled = true;
                return;
            }
            this._thr = new Thread( this.CalculatePrice );
            this._thr.Start();
        }
    }
}