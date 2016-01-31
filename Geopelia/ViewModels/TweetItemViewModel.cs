using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using CoreTweet.Streaming;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetItemViewModel : ViewModelBase
    {
        private readonly INavigationService _iNavigationService;
        private readonly TwitterClient      _tweetClient;

        private ReactiveProperty<TweetModel> _tweetModel = new ReactiveProperty<TweetModel>();
        public ReactiveProperty<TweetModel> TweetModel
        {
            get { return this._tweetModel; }
            set { this.SetProperty(ref this._tweetModel, value); }
        }

        public ReactiveProperty<Brush>      BorderBrush         = new ReactiveProperty<Brush>();
        public ReactiveProperty<string>     Brush               = new ReactiveProperty<string>();
        public ReactiveProperty<string>     FavoriteForground   = new ReactiveProperty<string>();
        public ReactiveProperty<string>     RetweetForground    = new ReactiveProperty<string>();
        public ReactiveProperty<string>     RetweetText         = new ReactiveProperty<string>();
        public ReactiveProperty<Visibility> TweetVisibility     = new ReactiveProperty<Visibility>();
        public ReactiveProperty<Visibility> ProtectedVisibility = new ReactiveProperty<Visibility>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iNavigationService"></param>
        /// <param name="s"></param>
        /// <param name="twitterClient"></param>
        public TweetItemViewModel(INavigationService iNavigationService, StatusMessage s, TwitterClient twitterClient)
        {
            this._iNavigationService       = iNavigationService;
            this._tweetClient              = twitterClient;
            this._tweetModel.Value         = new TweetModel(s);
            this.Brush.Value               = this.SetBorderBrushColor();
            this.TweetVisibility.Value     = this.GetVisibility();
            this.ProtectedVisibility.Value = this.GetProtectedVisibility();
            this.SetRetweetForegroundAndText();
            this.SetFavoriteForeground();
        }

        public void TappedEventHandler()
        {

        }

        /// <summary>
        /// リツイート状態を切替える
        /// </summary>
        public async void ChangeIsRetweeted()
        {
            var newIsRetweeted = (bool) !this.TweetModel.Value.TweetStatusMessage.Status.IsRetweeted;
            var statusResponse = await this._tweetClient.ChangeIsRetweeted(this.TweetModel.Value.Id, newIsRetweeted);
            this.TweetModel.Value.TweetStatusMessage.Status.IsRetweeted = statusResponse.IsRetweeted;
            this.SetRetweetForegroundAndText();
        }

        /// <summary>
        /// お気に入りに登録する
        /// </summary>
        public async void AddFavorite()
        {
            var statusResponse = await this._tweetClient.Favorite(this.TweetModel.Value.Id);
            this.TweetModel.Value.TweetStatusMessage.Status.IsFavorited = statusResponse.IsFavorited;
            this.SetFavoriteForeground();
        }

        public void NavigateTweetCreatePage()
        {
            this._iNavigationService.Navigate("TweetCreate", this.TweetModel.Value.Id);
        }

        /// <summary>
        /// ツイートの左ボーダー色を返却する
        /// </summary>
        /// <returns></returns>
        private string SetBorderBrushColor()
        {
            if (this.TweetModel.Value.RetweetedStatus != null)
            {
                return "DodgerBlue";
            }

            if (this.TweetModel.Value.Text.Contains("@tomoya_shibata"))
            {
                return "LimeGreen";
            }

            return "Transparent";
        }

        /// <summary>
        /// 鍵アイコンの Visibility を取得する
        /// </summary>
        /// <returns></returns>
        private Visibility GetProtectedVisibility()
        {
            return this.TweetModel.Value.TweetStatusMessage.Status.User.IsProtected ? Visibility.Visible
                                                                                    : Visibility.Collapsed;
        }

        /// <summary>
        /// 被リツイートユーザアイコンの Visibility を取得する
        /// </summary>
        /// <returns></returns>
        public Visibility GetVisibility()
        {
            return this.TweetModel.Value.RetweetedStatus != null ? Visibility.Visible
                                                                 : Visibility.Collapsed;
        }

        /// <summary>
        /// お気に入りボタンの色をセットする
        /// </summary>
        /// <returns></returns>
        public void SetFavoriteForeground()
        {
            this.FavoriteForground.Value = this.TweetModel.Value.TweetStatusMessage.Status.IsFavorited == true ? "Gold"
                                                                                                               : "White";
        }

        /// <summary>
        /// リツイートボタンの色をセットする
        /// </summary>
        /// <returns></returns>
        public void SetRetweetForegroundAndText()
        {
            if (this.TweetModel.Value.TweetStatusMessage.Status.IsRetweeted == true)
            {
                this.RetweetForground.Value = "LawnGreen";
                this.RetweetText.Value      = "リツイートを取り消す";
                return;
            }

            this.RetweetForground.Value = "White";
            this.RetweetText.Value      = "リツイートする";
        }
    }
}