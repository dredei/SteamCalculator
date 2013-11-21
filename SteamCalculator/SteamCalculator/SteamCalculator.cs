#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;

#endregion

namespace SteamCalculator
{
    public class UpdEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public int MaxProgress { get; set; }
        public string Msg { get; set; }
    }

    /// <summary>
    /// Class allows you to get all the games account: name, id and price.
    /// </summary>
    public class SteamCalculator
    {
        #region Classes

        public class GamesJsonGame
        {
            public int appid { get; set; }
            public int? playtime_forever { get; set; }
            public int? playtime_2weeks { get; set; }
        }

        public class GamesJSONResponse
        {
            public int game_count { get; set; }
            public List<GamesJsonGame> games { get; set; }
        }

        public class GamesJSON
        {
            public GamesJSONResponse response { get; set; }
        }

        public class Games
        {
            public string appId { get; set; }
            public string name { get; set; }
            public double price { get; set; }
            public string region = "us";
        }

        #endregion

        public event EventHandler<UpdEventArgs> updEvent = delegate { };

        private readonly string key;

        private const string UrlGetOwnedGames =
            "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json";

        private const string UrlGame = "http://store.steampowered.com/app/{0}/?cc=us";
        private const string UrlProfInfo = "http://steamcommunity.com/id/{0}/?xml=1&l=english";

        /// <summary>
        /// Default constructor with my API-key.
        /// </summary>
        public SteamCalculator()
            : this( "B19695B79BF8679540CA66B25D292D1F" )
        {
        }

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
            var client = new WebClient();
            Stream stream = client.OpenRead( url );
            var reader = new StreamReader( stream );
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Get game price.
        /// </summary>
        /// <param name="content">Game page (HTML).</param>
        /// <returns>Price</returns>
        public double GetGamePrice( string content )
        {
            double price = double.NaN;
            var regex =
                new Regex(
                    @"<div\sclass=""game_purchase_price\sprice""\s*itemprop=""price"">\s*&#36;([0-9.]+)(\sUSD)?\s*</div>" );
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
        public string GetGameName( string content )
        {
            string name = string.Empty;
            var regex = new Regex( @"<div class=""apphub_AppName"">([^<>]*)</div>" );
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
        public void GetGameInfo( string appId, out double price, out string name )
        {
            string content = navigate( UrlGame.F( appId ) );
            price = GetGamePrice( content );
            name = GetGameName( content );
        }

        /// <summary>
        /// Get Commmunity ID by ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Community ID.</returns>
        public string GetCommunityId( string id )
        {
            var xmlDoc = new XmlDocument();
            string xml = navigate( UrlProfInfo.F( id ) );
            xmlDoc.LoadXml( xml );
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes( "profile" );
            return xmlNodeList[ 0 ].ChildNodes[ 0 ].InnerText;
        }

        /// <summary>
        /// Get games account (Steam Web API) with AppId.
        /// </summary>
        /// <param name="communityId">CommunityId</param>
        /// <returns>Games list.</returns>
        public List<GamesJsonGame> getGamesJSON( string communityId )
        {
            var gamesList = new List<GamesJsonGame>();
            string games = navigate( UrlGetOwnedGames.F( key, communityId ) );
            var gj = JsonConvert.DeserializeObject<GamesJSON>( games );
            GamesJSONResponse gjResponse = gj.response;
            if ( gjResponse.game_count == 0 )
            {
                throw new Exception( "Make your profile public!" );
            }
            for ( int i = 0; i < gjResponse.games.Count; i++ )
            {
                var gjg = new GamesJsonGame();
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
        /// <param name="communityId">CommunityId</param>
        /// <returns>Games list with names, price and AppId.</returns>
        public List<Games> GetGames( string communityId )
        {
            var games = new List<Games>();
            var regex = new Regex( "^[0-9]+$" );
            Match match = regex.Match( communityId );
            if ( !match.Success )
            {
                communityId = GetCommunityId( communityId );
            }
            var gamesJson = getGamesJSON( communityId );
            var args = new UpdEventArgs();
            args.MaxProgress = gamesJson.Count;
            args.Progress = 0;
            for ( int i = 0; i < gamesJson.Count; i++ )
            {
                var game = new Games();
                double price;
                string name = string.Empty;
                GetGameInfo( gamesJson[ i ].appid.ToString(), out price, out name );
                name = WebUtility.HtmlDecode( name );
                game.appId = gamesJson[ i ].appid.ToString();
                game.price = price;
                game.name = name;
                if ( !string.IsNullOrEmpty( name ) )
                {
                    games.Add( game );

                    args.Progress++;
                    args.Msg = "{0};{1}".F( name, price );
                    updEvent( this, args );
                }
                else
                {
                    args.Progress++;
                    args.Msg = "";
                    updEvent( this, args );
                }
            }
            return games;
        }
    }

    public static class ExtensionsMethods
    {
        public static string F( this string format, params object[] args )
        {
            return String.Format( format, args );
        }
    }
}