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
    public partial class frmMain : Form
    {
        public string guardCode = string.Empty;

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            List<SteamCalculator.games> gamesList = new List<SteamCalculator.games>();
            SteamCalculator steamCalc = new SteamCalculator();
            //gamesList = steamCalc.getGamesJSON( "76561197960434622" );
            gamesList = steamCalc.getGames( "76561198013216436" );
            string f = "f";
        }
    }
}
