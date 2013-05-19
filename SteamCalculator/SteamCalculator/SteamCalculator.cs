using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace SteamCalculator
{
    public class UpdEventArgs : EventArgs
    {
        public int progress { get; set; }
        public int maxProgress { get; set; }
        public string msg { get; set; }
    }

    /// <summary>
    /// Class allows you to get all the games account: name, id and price.
    /// </summary>
    class SteamCalculator
    {
        public class gamesJSONGame
        {
            public int appid { get; set; }
            public int? playtime_forever { get; set; }
            public int? playtime_2weeks { get; set; }
        }

        public class gamesJSONResponse
        {
            public int game_count { get; set; }
            public List<gamesJSONGame> games { get; set; }
        }

        public class gamesJSON
        {
            public gamesJSONResponse response { get; set; }
        }

        public class games
        {
            public string appId { get; set; }
            public string name { get; set; }
            public double price { get; set; }
            public string region = "us";
        }

        string key;
        const string urlGetOwnedGames = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json";
        const string urlGame = "http://store.steampowered.com/app/{0}/?cc=us";
        public event EventHandler<UpdEventArgs> updEvent = delegate { };

        /// <summary>
        /// Default constructor with my API-key.
        /// </summary>
        public SteamCalculator()
            : this( "B19695B79BF8679540CA66B25D292D1F" )
        { }

        /// <summary>
        /// Constructor with your API-key.
        /// </summary>
        /// <param name="key">API-key</param>
        public SteamCalculator( string key )
        {
            this.key = key;
        }

        /// <summary>
        /// Opens the page.
        /// </summary>
        /// <param name="url">Page url.</param>
        /// <returns>Page content.</returns>
        public string navigate( string url )
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead( url );
            StreamReader reader = new StreamReader( stream );
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Get game price.
        /// </summary>
        /// <param name="content">Game page (HTML).</param>
        /// <returns>Price</returns>
        public double getGamePrice( string content )
        {
            double price = double.NaN;
            Regex regex = new Regex( @"<div\sclass=""game_purchase_price\sprice""\s*itemprop=""price"">\s*&#36;([0-9.]+)(\sUSD)?\s*</div>" );
            Match match = regex.Match( content );
            if ( !match.Success )
            {
                regex = new Regex( @"<div\sclass=""discount_original_price"">&#36;([0-9.]+)(\sUSD)?</div>" );
                match = regex.Match( content );
                string s = match.Groups[ 1 ].ToString().Replace( '.', ',' );
                if ( !match.Success )
                {
                    price = 0;
                }
                else
                {
                    price = double.Parse( s );
                }
            }
            else
            {
                string s = match.Groups[ 1 ].ToString().Replace( '.', ',' );
                price = double.Parse( s );
            }
            return price;
        }

        /// <summary>
        /// Get game name.
        /// </summary>
        /// <param name="content">Game page (HTML).</param>
        /// <returns>Game name.</returns>
        public string getGameName( string content )
        {
            string name = string.Empty;
            Regex regex = new Regex( @"<div class=""apphub_AppName"">([^<>]*)</div>" );
            Match match = regex.Match( content );
            name = match.Groups[ 1 ].ToString();
            return name;
        }

        /// <summary>
        /// Get game info (price and name).
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="price">out price</param>
        /// <param name="name">out price</param>
        public void getGameInfo( string appId, out double price, out string name )
        {
            string content = navigate( urlGame.f( appId ) );
            price = getGamePrice( content );
            name = getGameName( content );
        }

        /// <summary>
        /// Get games account (Steam Web API) with AppId.
        /// </summary>
        /// <param name="steamId">SteamId</param>
        /// <returns>Games list.</returns>
        public List<gamesJSONGame> getGamesJSON( string steamId )
        {
            List<gamesJSONGame> gamesList = new List<gamesJSONGame>();
            string games = navigate( urlGetOwnedGames.f( key, steamId ) );
            gamesJSON gj = JsonConvert.DeserializeObject<gamesJSON>( games );
            gamesJSONResponse gjResponse = gj.response;
            if ( gjResponse.game_count == 0 )
            {
                throw new Exception( "Make your profile public!" );
            }
            for ( int i = 0; i < gjResponse.games.Count; i++ )
            {
                gamesJSONGame gjg = new gamesJSONGame();
                gjg.appid = gjResponse.games[ i ].appid;
                if ( gjResponse.games[ i ].playtime_2weeks == null )
                {
                    gjg.playtime_2weeks = 0;
                }
                else
                {
                    gjg.playtime_2weeks = gjResponse.games[ i ].playtime_2weeks;
                }
                if ( gjResponse.games[ i ].playtime_forever == null )
                {
                    gjg.playtime_forever = 0;
                }
                else
                {
                    gjg.playtime_forever = gjResponse.games[ i ].playtime_forever;
                }
                gamesList.Add( gjg );
            }
            return gamesList;
        }

        /// <summary>
        /// Get games with names, price and AppId.
        /// </summary>
        /// <param name="steamId">SteamId</param>
        /// <returns>Games list with names, price and AppId.</returns>
        public List<games> getGames( string steamId )
        {
            List<games> games = new List<games>();
            List<gamesJSONGame> gamesJSON = getGamesJSON( steamId );
            UpdEventArgs args = new UpdEventArgs();
            args.maxProgress = gamesJSON.Count;
            args.progress = 0;
            for ( int i = 0; i < gamesJSON.Count; i++ )
            {
                games game = new games();
                double price = double.NaN;
                string name = string.Empty;
                getGameInfo( gamesJSON[ i ].appid.ToString(), out price, out name );
                name = WebUtility.HtmlDecode( name );
                game.appId = gamesJSON[ i ].appid.ToString();
                game.price = price;
                game.name = name;
                if ( !string.IsNullOrEmpty( name ) )
                {
                    games.Add( game );

                    args.progress++;
                    args.msg = "{0};{1}".f( name, price );
                    updEvent( this, args );
                }
                else
                {
                    args.progress++;
                    args.msg = "";
                    updEvent( this, args );
                }

                //Thread.Sleep( 100 );
            }
            return games;
        }
    }

    public static class ExtensionsMethods
    {
        public static string f( this string format, params object[] args )
        {
            return String.Format( format, args );
        }
    }
}
