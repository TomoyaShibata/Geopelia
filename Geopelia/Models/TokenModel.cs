using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using CoreTweet;
using Newtonsoft.Json;
using Prism.Mvvm;
using System.Reactive.Linq;

namespace Geopelia.Models
{
    public class TokenModel : BindableBase
    {
        private ObservableCollection<Tokens> _tokensList = new ObservableCollection<Tokens>();
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
        }

        /// <summary>
        /// 新規ユーザの認証をする
        /// </summary>
        public async Task<Tokens> AuthorizeNewUserAsync()
        {
            var session                 = await OAuth.AuthorizeAsync(TwitterConst.ConsumerKey, TwitterConst.ConsumerSecret, "https://twitter.com/tomoya_shibata/callback");
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, session.AuthorizeUri, new Uri("https://twitter.com/tomoya_shibata/callback"));

            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.UserCancel) return null;

            var oauthVerifier = new WwwFormUrlDecoder(new Uri(webAuthenticationResult.ResponseData).Query).GetFirstValueByName("oauth_verifier");
            var tokens = await session.GetTokensAsync(oauthVerifier);
            SaveTokens(tokens);
            return tokens;
        }

        /// <summary>
        /// 認証情報を保存する
        /// </summary>
        private static void SaveTokens(Tokens tokens)
        {
            var key               = $"{tokens.ConsumerSecret}:{tokens.UserId}";
            var authorizationInfo = new AuthorizationInfoModel
            {
                ScreenName        = tokens.ScreenName,
                ConsumerKey       = tokens.ConsumerKey,
                ConsumerSecret    = tokens.ConsumerSecret,
                AccessToken       = tokens.AccessToken,
                AccessTokenSecret = tokens.AccessTokenSecret,
                DisplayOrder      = GetGeopeliaAccountCount()
            };

            var json = JsonConvert.SerializeObject(authorizationInfo);
            new PasswordVault().Add(new PasswordCredential("GeopeliaAccount", key, json));
        }

        /// <summary>
        /// 既存の認証情報を読み込む
        /// </summary>
        private static IEnumerable<Tokens> LoadTokens()
        {
            return new PasswordVault().RetrieveAll()
                                      .Where(c => c.Resource == "GeopeliaAccount")
                                      .Select(c =>
            {
                var passwordCredential = new PasswordVault().Retrieve(c.Resource, c.UserName);
                var authorizeInfo      = JsonConvert.DeserializeObject<AuthorizationInfoModel>(passwordCredential.Password);
                return CreateTokens(authorizeInfo);
            });
        }

        /// <summary>
        /// 認証情報を全て削除する
        /// </summary>
        public async void RemoveAllTokens()
        {
            var passwordValut = new PasswordVault();
            await passwordValut.RetrieveAll()
                               .ToObservable()
                               .Where(c => c.Resource == "GeopeliaAccount")
                               .ForEachAsync(c => passwordValut.Remove(c));
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
        /// <param name="authorizationInfo">認証情報</param>
        private static Tokens CreateTokens(AuthorizationInfoModel authorizationInfo)
            => Tokens.Create(authorizationInfo.ConsumerKey, authorizationInfo.ConsumerSecret, authorizationInfo.AccessToken, authorizationInfo.AccessTokenSecret);
    }
}
