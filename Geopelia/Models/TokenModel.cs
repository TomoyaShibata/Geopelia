using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using CoreTweet;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Geopelia.Models
{
    public class TokenModel : BindableBase
    {
        private ObservableCollection<Tokens> _tokensList;
        public ObservableCollection<Tokens> TokensList
        {
            get { return this._tokensList; }
            set { this.SetProperty(ref this._tokensList, value); }
        }

        public TokenModel()
        {
            this.TokensList = new ObservableCollection<Tokens>(LoadTokens().ToList());
            if (TokensList.Count != 0) return;
            //var passwordVault = new PasswordVault();
            //foreach (var credential in passwordVault.RetrieveAll().Where(credential => credential.Resource == "GeopeliaAccount"))
            //{
            //    passwordVault.Remove(credential);
            //}
            this.AuthorizeNewUserAsync();
        }

        /// <summary>
        /// 新規ユーザの認証をする
        /// </summary>
        private async void AuthorizeNewUserAsync()
        {
            var session                 = await OAuth.AuthorizeAsync(TwitterConst.ConsumerKey, TwitterConst.ConsumerSecret, "https://twitter.com/tomoya_shibata/callback");
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, session.AuthorizeUri, new Uri("https://twitter.com/tomoya_shibata/callback"));

            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.UserCancel) return;

            var oauthVerifier = new WwwFormUrlDecoder(new Uri(webAuthenticationResult.ResponseData).Query).GetFirstValueByName("oauth_verifier");
            var tokens        = await session.GetTokensAsync(oauthVerifier);
            SaveTokens(tokens);
        }

        /// <summary>
        /// 認証情報を保存する
        /// </summary>
        private static void SaveTokens(Tokens tokens)
        {
            var key = $"{tokens.ConsumerSecret}:{tokens.UserId}";
            var dictionary = new Dictionary<string, object>
            {
                { "screenName"       , tokens.ScreenName },
                { "consumerKey"      , tokens.ConsumerKey },
                { "consumerSecret"   , tokens.ConsumerSecret },
                { "AccessToken"      , tokens.AccessToken },
                { "AccessTokenSecret", tokens.AccessTokenSecret},
                { "displayOrder"     , GetGeopeliaAccountCount() }
            };
            var passwordVault = new PasswordVault();
            var json = JsonConvert.SerializeObject(dictionary);
            passwordVault.Add(new PasswordCredential("GeopeliaAccount", key, json));
        }

        /// <summary>
        /// 既存の認証情報を読み込む
        /// </summary>
        private static IEnumerable<Tokens> LoadTokens()
        {
            return new PasswordVault().RetrieveAll().Where(c => c.Resource == "GeopeliaAccount").Select(c =>
            {
                var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(c.Password);
                var consumerKey       = deserializeObject["ConsumerKey"].ToString();
                var consumerSecret    = deserializeObject["consumerSecret"].ToString();
                var accessToken       = deserializeObject["AccessToken"].ToString();
                var accessTokenSecret = deserializeObject["AccessTokenSecret"].ToString();
                return CreateTokens(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            });
        }

        /// <summary>
        /// 既存の認証情報の数を取得する
        /// </summary>
        /// <returns></returns>
        private static int GetGeopeliaAccountCount()
            => new PasswordVault().RetrieveAll().Count(c => c.Resource == "GeopeliaAccount");

        /// <summary>
        /// トークンを生成する
        /// </summary>
        /// <param name="consumerKey">ConsumerKey</param>
        /// <param name="consumerSecret">ConsumerSecret</param>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="accessTokenSecret">AccessTokenSecret</param>
        private static Tokens CreateTokens(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
            => Tokens.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret);
    }
}
