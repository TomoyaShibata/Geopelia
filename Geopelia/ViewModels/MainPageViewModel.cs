using System;
using CoreTweet;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public int PivotItemWidth { get; set; } = 200;

        private string _nowDateTime = DateTime.Now.ToString();
        public  string NowDateTime
        {
            get { return this._nowDateTime; }
            set { this.SetProperty(ref this._nowDateTime, value); }
        }

        public ReactiveProperty<string>       TweetText    { get; set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<UserResponse> MyProfile    { get; set; } = new ReactiveProperty<UserResponse>();
        public ReactiveProperty<Uri>          ProfileImage { get; set; } = new ReactiveProperty<Uri>();
        private readonly TwitterClient _twitterClient = new TwitterClient();

        public MainPageViewModel()
        {
            this.GetMyProfile();
        }

        /// <summary>
        /// Tweet を Post する
        /// </summary>
        public void PostTweet()
        {
            this._twitterClient.PostTweet(this.TweetText.Value);
        }

        /// <summary>
        /// 自分のプロフィールを取得する
        /// </summary>
        private void GetMyProfile()
        {
            this.MyProfile.Value    = this._twitterClient.GetMyProfile();
            this.ProfileImage.Value = new Uri(this.MyProfile.Value.ProfileImageUrlHttps);
        }
    }
}
