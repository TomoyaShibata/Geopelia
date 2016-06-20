using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using CoreTweet;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

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
        public ReactiveProperty<bool?> IsShowSplitView { get; set; } = new ReactiveProperty<bool?>(false);

        public class ItemModelBase { }
        private readonly TwitterClient _twitterClient;

        public MainPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this._twitterClient    = twitterClient;
            this.NavigationService = navigationService;

            this.Width.Value = Window.Current.Bounds.Width;

            this.GetMyProfile();

            this.Timelines    = this._twitterClient.Timelines.ToReadOnlyReactiveCollection();
            this.TweetItems   = this._twitterClient.TweetItems.ToReadOnlyReactiveCollection();
            this.MentionItems = this._twitterClient.MentionItems.ToReadOnlyReactiveCollection();

            this._twitterClient.InitTimelines(this.NavigationService);
            this._twitterClient.InitMentions(this.NavigationService);
            this.StartStreaming();
            this.ho();
        }

        /// <summary>
        /// Streaming 受信を開始する
        /// </summary>
        public void StartStreaming()
        {
            this._twitterClient.StartStreaming(this.NavigationService);
        }

        private void ho()
        {
            this.MentionItems.ObserveAddChangedItems().Subscribe(x =>
            {
                //var template = ToastTemplateType.ToastText01;
                //var toastXml = ToastNotificationManager.GetTemplateContent(template);
                //var textTag  = toastXml.GetElementsByTagName("text").First();
                //textTag.AppendChild(toastXml.CreateTextNode(x[0].TweetModel.Value.Text));
                //var notifier = ToastNotificationManager.CreateToastNotifier();
                //var notify   = new ToastNotification(toastXml);
                //notify.Tag   = "Retwet";
                //notifier.Show(notify);
            });
        }

        /// <summary>
        /// 自分のプロフィールを取得する
        /// </summary>
        private async void GetMyProfile()
        {
            this.MyProfile.Value    = await this._twitterClient.GetMyProfile();
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

        /// <summary>
        /// ツイート詳細画面に遷移する
        /// </summary>
        public void NavigateTweetDetailsPage()
        {
            this.NavigationService.Navigate("TweetDetails", null);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            this._twitterClient.ReplyToStatus = null;
        }
    }
}
