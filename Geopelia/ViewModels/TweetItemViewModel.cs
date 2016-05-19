﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using CoreTweet;
using Geopelia.Models;
using Microsoft.Practices.ObjectBuilder2;
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

        public ReactiveProperty<Brush>      BorderBrush                = new ReactiveProperty<Brush>();
        public ReactiveProperty<string>     Brush                      = new ReactiveProperty<string>();
        public ReactiveProperty<string>     FavoriteForground          = new ReactiveProperty<string>();
        public ReactiveProperty<string>     RetweetForground           = new ReactiveProperty<string>();
        public ReactiveProperty<string>     RetweetText                = new ReactiveProperty<string>();
        public ReactiveProperty<Visibility> TweetVisibility            = new ReactiveProperty<Visibility>();
        public ReactiveProperty<Visibility> ProtectedVisibility        = new ReactiveProperty<Visibility>();
        public ReactiveProperty<string>     ReplyToTweetText           = new ReactiveProperty<string>();
        public ReactiveProperty<Visibility> ReplyToTweetTextVisibility = new ReactiveProperty<Visibility>();
        public ReactiveProperty<Visibility> PictureVisibility          = new ReactiveProperty<Visibility>();
        public ReactiveCollection<string>   Urls                       = new ReactiveCollection<string>();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iNavigationService"></param>
        /// <param name="s"></param>
        /// <param name="twitterClient"></param>
        public TweetItemViewModel(INavigationService iNavigationService, Status s, TwitterClient twitterClient)
        {
            this._iNavigationService       = iNavigationService;
            this._tweetClient              = twitterClient;
            this._tweetModel.Value         = new TweetModel(s);
            this.Brush.Value               = this.SetBorderBrushColor();
            this.TweetVisibility.Value     = this.GetVisibility();
            this.ProtectedVisibility.Value = this.GetProtectedVisibility();
            this.SetPictureVisibility();
            this.SetRetweetForegroundAndText();
            this.SetFavoriteForeground();
            this.SetReplyToTweetText();
        }

        public void TappedEventHandler()
        {

        }

        /// <summary>
        /// リツイート状態を切替える
        /// </summary>
        public async void ChangeIsRetweeted()
        {
            var newIsRetweeted = (bool) !this.TweetModel.Value.TweetStatus.IsRetweeted;
            var statusResponse = await this._tweetClient.ChangeIsRetweetedAsync(this.TweetModel.Value, newIsRetweeted);

            if (newIsRetweeted)
            {
                this.TweetModel.Value.MyRetweetId = statusResponse.Id;
            }

            this.TweetModel.Value.TweetStatus.IsRetweeted = newIsRetweeted;
            this.SetRetweetForegroundAndText();
        }

        /// <summary>
        /// お気に入り状態を切替える
        /// </summary>
        public async void ChangeIsFavorited()
        {
            var newIsFavorited = (bool) !this.TweetModel.Value.TweetStatus.IsFavorited;
            var statusResponse = await this._tweetClient.ChangeIsFavorited(this.TweetModel.Value.Id, newIsFavorited);
            this.TweetModel.Value.TweetStatus.IsFavorited = statusResponse.IsFavorited;
            this.SetFavoriteForeground();
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

            return this.TweetModel.Value.Text.Contains("@tomoya_shibata") ? "LimeGreen"
                                                                          : "Transparent";
        }

        /// <summary>
        /// 鍵アイコンの Visibility を取得する
        /// </summary>
        /// <returns></returns>
        private Visibility GetProtectedVisibility() => this.TweetModel.Value.TweetStatus.User.IsProtected ? Visibility.Visible
                                                                                                          : Visibility.Collapsed;

        /// <summary>
        /// 被リツイートユーザアイコンの Visibility を取得する
        /// </summary>
        /// <returns></returns>
        public Visibility GetVisibility() => this.TweetModel.Value.RetweetedStatus != null ? Visibility.Visible
                                                                                           : Visibility.Collapsed;

        /// <summary>
        /// お気に入りボタンの色をセットする
        /// </summary>
        /// <returns></returns>
        public void SetFavoriteForeground()
        {
            this.FavoriteForground.Value = this.TweetModel.Value.TweetStatus.IsFavorited == true ? "Pink"
                                                                                                 : "White";
        }

        /// <summary>
        /// リツイートボタンの色をセットする
        /// </summary>
        /// <returns></returns>
        public void SetRetweetForegroundAndText()
        {
            if (this.TweetModel.Value.TweetStatus.IsRetweeted == true)
            {
                this.RetweetForground.Value = "LawnGreen";
                this.RetweetText.Value      = "リツイートを取り消す";
                return;
            }

            this.RetweetForground.Value = "White";
            this.RetweetText.Value      = "リツイートする";
        }

        /// <summary>
        /// リプライ先のツイートテキストをセットする
        /// </summary>
        private async void SetReplyToTweetText()
        {
            var inReplyToStatusId = this.TweetModel.Value.TweetStatus.InReplyToStatusId;
            if (inReplyToStatusId == null)
            {
                this.ReplyToTweetTextVisibility.Value = Visibility.Collapsed;
                return;
            }

            this.ReplyToTweetTextVisibility.Value    = Visibility.Visible;
            this.TweetModel.Value.ReplyStatusMessage = await this._tweetClient.GetTweetAsync((long)inReplyToStatusId);
            this.ReplyToTweetText.Value              = this.TweetModel.Value.ReplyStatusMessage.Text;
        }

        private void SetPictureVisibility()
        {
            this.PictureVisibility.Value = this.TweetModel.Value.PicTwitterUris == null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void hoge()
        {
            this.TweetModel.Value.TweetStatus.Entities.Urls.ForEach(u => this.Urls.Add(u.Url));
        }

        public void TextBlock_Loaded(object sender, RoutedEventArgs args)
        {
            var textBlock        = sender as TextBlock;
            var run              = new Run {Text = "クラりん可愛い"};
            var hyperlink        = new Hyperlink { NavigateUri = new Uri("https://www.google.co.jp/")};
            hyperlink.Inlines.Add(run);
            textBlock.Inlines.Add(hyperlink);
        }

        /// <summary>
        /// 返信画面に遷移する
        /// </summary>
        public void NavigateTweetCreatePage()
        {
            this._tweetClient.ReplyToStatus = this.TweetModel.Value.TweetStatus;
            this._iNavigationService.Navigate("TweetCreate", this._tweetClient);
        }

        /// <summary>
        /// ユーザ画面に遷移する
        /// </summary>
        public void NavigateUserPage()
        {
            this._iNavigationService.Navigate("User", null);
        }

        /// <summary>
        /// ツイート詳細画面に遷移する
        /// </summary>
        public void NavigateTweetDetailsPage()
        {
            this._tweetModel.Value.IsSelected = true;
            this._iNavigationService.Navigate("TweetDetails", null);
        }

        /// <summary>
        /// ハッシュタグツイート一覧画面に遷移する
        /// </summary>
        public void NavigateHashtagTweetsPage()
        {
            this._iNavigationService.Navigate("HashtagTweets", null);
        }
    }
}