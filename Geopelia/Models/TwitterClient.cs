using System.Threading.Tasks;
using CoreTweet;
using Prism.Mvvm;

namespace Geopelia.Models
{
    internal class TwitterClient : BindableBase
    {
        private readonly Tokens _tokens;
        private readonly long   _userId;

        public TwitterClient()
        {
            this._tokens = Tokens.Create(TwitterConst.ConsumerKey, TwitterConst.ConsumerSecret, TwitterConst.AccessToken,
                TwitterConst.AccessTokenSecret);
        }

        public void PostTweet(string s)
        {
            this._tokens.Statuses.UpdateAsync(new { status = s });
        }

        public UserResponse GetMyProfile()
        {
            return this._tokens.Users.ShowAsync(57864731).Result;
        }
    }
}
