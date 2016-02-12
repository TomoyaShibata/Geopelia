using System;
using Windows.UI.Xaml;
using CoreTweet;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }


        public int PivotItemWidth { get; set; } = 400;

        private string _nowDateTime = DateTime.Now.ToString();
        public string NowDateTime
        {
            get { return this._nowDateTime; }
            set { this.SetProperty(ref this._nowDateTime, value); }
        }

        public ReactiveProperty<string> TweetText { get; set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<UserResponse> MyProfile { get; set; } = new ReactiveProperty<UserResponse>();
        public ReactiveProperty<Uri> ProfileImage { get; set; } = new ReactiveProperty<Uri>();
        public ReadOnlyReactiveCollection<TweetModel> Timelines { get; set; }
        public ReadOnlyReactiveCollection<TweetItemViewModel> TweetItems { get; set; }
        public ReadOnlyReactiveCollection<TweetItemViewModel> MentionItems { get; set; }
        public ReactiveProperty<double> Width { get; set; } = new ReactiveProperty<double>();

        private readonly TwitterClient _twitterClient;

        public MainPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this._twitterClient    = twitterClient;
            this.NavigationService = navigationService;

            this.Width.Value = Window.Current.Bounds.Width;

            this.GetMyProfile();

            this.Timelines = this._twitterClient.Timelines.ToReadOnlyReactiveCollection();
            this.TweetItems = this._twitterClient.TweetItems.ToReadOnlyReactiveCollection();
            this.MentionItems = this._twitterClient.MentionItems.ToReadOnlyReactiveCollection();

            this.StartStreaming();
        }

        /// <summary>
        /// Streaming 受信を開始する
        /// </summary>
        public void StartStreaming()
        {
            this._twitterClient.StartStreaming(this.NavigationService);
        }

        /// <summary>
        /// 自分のプロフィールを取得する
        /// </summary>
        private void GetMyProfile()
        {
            this.MyProfile.Value    = this._twitterClient.GetMyProfile();
            this.ProfileImage.Value = new Uri(this.MyProfile.Value.ProfileImageUrlHttps);
        }
	
	    /// <summary>
        /// ツイート一覧の現在位置を上部にジャンプする
        /// </summary>
        public void JumpToTop()
        {
        }


        public void NavigateNextPage()
        {
            this.NavigationService.Navigate("TweetCreate", null);
        }
    }
}
