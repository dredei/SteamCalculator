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

        public SteamCalculator()
            : this( "B19695B79BF8679540CA66B25D292D1F" )
        { }

        public SteamCalculator( string key )
        {
            this.key = key;
        }

        public string navigate( string url )
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead( url );
            StreamReader reader = new StreamReader( stream );
            return reader.ReadToEnd();
        }

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

        public string getGameName( string content )
        {
            string name = string.Empty;
            Regex regex = new Regex( @"<div class=""apphub_AppName"">([^<>]*)</div>" );
            Match match = regex.Match( content );
            name = match.Groups[ 1 ].ToString();            
            return name;
        }

        public void getGameInfo( string appId, out double price, out string name )
        {            
            string content = navigate( urlGame.f( appId ) );
            price = getGamePrice( content );
            name  = getGameName( content );
        }

        public List<gamesJSONGame> getGamesJSON( string steamId )
        {
            List<gamesJSONGame> gamesList = new List<gamesJSONGame>();
            string games = navigate( urlGetOwnedGames.f( key, steamId ) );
            gamesJSON gj = JsonConvert.DeserializeObject<gamesJSON>( games );
            gamesJSONResponse gjResponse = gj.response;
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

        public List<games> getGames( string steamId )
        {
            List<games> games = new List<games>();
            List<gamesJSONGame> gamesJSON = getGamesJSON( steamId );
            for ( int i = 0; i < gamesJSON.Count; i++ )
            {
                games game = new games();
                double price = double.NaN;
                string name = string.Empty;
                getGameInfo( gamesJSON[ i ].appid.ToString(), out price, out name );
                game.appId = gamesJSON[ i ].appid.ToString();
                game.price = price;
                game.name = name;
                games.Add( game );
                Thread.Sleep( 100 );
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
