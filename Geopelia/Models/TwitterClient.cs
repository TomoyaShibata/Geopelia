using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CoreTweet;
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

        //private ObservableCollection<TweetModel> _mentions = new ObservableCollection<TweetModel>();
        //public ObservableCollection<TweetModel> Mentions
        //{
        //    get { return this._mentions; }
        //    set { this.SetProperty(ref this._mentions, value); }
        //}

        public TwitterClient()
        {
            this._tokens = Tokens.Create(TwitterConst.ConsumerKey, TwitterConst.ConsumerSecret, TwitterConst.AccessToken,
                TwitterConst.AccessTokenSecret);
        }

        public void PostTweet(string s)
        {
            var updateAsync = this._tokens.Statuses.UpdateAsync(new { status = s });
        }

        public async Task<StatusResponse> ChangeIsRetweeted(long id, bool newIsRetweeted)
        {
            return newIsRetweeted ? await this._tokens.Statuses.RetweetAsync(id)
                                  : await this._tokens.Statuses.DestroyAsync(id);
        }

        /// <summary>
        /// お気に入りに追加する
        /// </summary>
        /// <param name="id">ツイート ID</param>
        public async Task<StatusResponse> Favorite(long id)
        {
            return await this._tokens.Favorites.CreateAsync(id);
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

        //public void StartStreamingMentions()
        //{
        //    this._tokens.Streaming.UserAsObservable()
        //        .Where(m => m.Type == MessageType.Create)
        //        .Cast<StatusMessage>()
        //        .Where(m => m.Status.InReplyToScreenName == "tomoya_shibata")
        //        .Subscribe(m => this._mentions.Add(new TweetModel(m)));
        //}

        public UserResponse GetMyProfile()
        {
            return this._tokens.Users.ShowAsync(57864731).Result;
        }

        public StatusResponse GetTweet(long id)
        {
            return _tokens.Statuses.ShowAsync(id).Result;
        }
    }
}
