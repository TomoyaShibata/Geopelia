using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CoreTweet;
using CoreTweet.Core;
using CoreTweet.Streaming;
using Geopelia.ViewModels;
using Prism.Mvvm;
using Prism.Windows.Navigation;

namespace Geopelia.Models
{
    public class TwitterClient : BindableBase
    {
        private readonly Tokens _tokens;

        private ObservableCollection<TweetModel> _timeLines = new ObservableCollection<TweetModel>();
        public ObservableCollection<TweetModel> Timelines
        {
            get { return this._timeLines; }
            set { this.SetProperty(ref this._timeLines, value); }
        }

        private ObservableCollection<TweetItemViewModel> _tweetItems = new ObservableCollection<TweetItemViewModel>();
        public ObservableCollection<TweetItemViewModel> TweetItems
        {
            get { return this._tweetItems; }
            set { this.SetProperty(ref this._tweetItems, value); }
        }

        public ObservableCollection<TweetItemViewModel> MentionItems = new ObservableCollection<TweetItemViewModel>();

        public TwitterClient()
        {
            this._tokens = Tokens.Create(TwitterConst.ConsumerKey, TwitterConst.ConsumerSecret, TwitterConst.AccessToken,
                TwitterConst.AccessTokenSecret);
        }

        public void PostTweet(string s)
        {
            var updateAsync = this._tokens.Statuses.UpdateAsync(new { status = s });
        }

        /// <summary>
        /// リツイート状態を切替える
        /// </summary>
        /// <param name="id">ツイート ID</param>
        /// <param name="newIsRetweeted"></param>
        /// <returns></returns>
        public async Task<StatusResponse> ChangeIsRetweeted(long id, bool newIsRetweeted)
        {
            return newIsRetweeted ? await this._tokens.Statuses.RetweetAsync(id)
                                  : await this._tokens.Statuses.DestroyAsync(id);
        }

        /// <summary>
        /// お気に入り状態を切替える
        /// </summary>
        /// <param name="id">ツイート ID</param>
        /// <param name="newIsFavorited"></param>
        public async Task<StatusResponse> ChangeIsFavorited(long id, bool newIsFavorited)
        {
            return newIsFavorited ? await this._tokens.Favorites.CreateAsync(id)
                                  : await this._tokens.Favorites.DestroyAsync(id);
        }

        public void StartStreaming(INavigationService iNavigationService)
        {
            var observable = this._tokens.Streaming.UserAsObservable()
                .Where(m => m.Type == MessageType.Create);
            observable
                .Cast<StatusMessage>()
                .Subscribe(m => this.TweetItems.Insert(0, new TweetItemViewModel(iNavigationService, m, this)));
            observable
                .Cast<StatusMessage>()
                .Where(m => m.Status.InReplyToScreenName?.Contains("tomoya_shibata") ?? false)
                .Subscribe(m => this.MentionItems.Insert(0, new TweetItemViewModel(iNavigationService, m, this)));
        }

        public UserResponse GetMyProfile()
        {
            return this._tokens.Users.ShowAsync(57864731).Result;
        }

        /// <summary>
        /// ツイートを取得する
        /// </summary>
        /// <param name="id">ツイートID</param>
        /// <returns>ツイート</returns>
        public StatusResponse GetTweet(long id)
        {
            return _tokens.Statuses.ShowAsync(id).Result;
        }
    }
}
