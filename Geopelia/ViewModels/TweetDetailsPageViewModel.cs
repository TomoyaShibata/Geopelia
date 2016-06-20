using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Geopelia.Models;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetDetailsPageViewModel : TransitedViewModelBase
    {
        public ReactiveProperty<TweetModel> TweetModel = new ReactiveProperty<TweetModel>();

        public ReactiveProperty<string> RetweetButtonForeground = new ReactiveProperty<string>();
        public ReactiveProperty<string> RetweetButtonText       = new ReactiveProperty<string>();

        public ReactiveProperty<Visibility> IsImages1Page = new ReactiveProperty<Visibility>(Visibility.Collapsed);
        public ReactiveProperty<Visibility> IsImages2Page = new ReactiveProperty<Visibility>(Visibility.Collapsed);
        public ReactiveProperty<Visibility> IsImages3Page = new ReactiveProperty<Visibility>(Visibility.Collapsed);
        public ReactiveProperty<Visibility> IsImages4Page = new ReactiveProperty<Visibility>(Visibility.Collapsed);

        public TweetDetailsPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this.NavigationService = navigationService;
            this.TwitterClient     = twitterClient;
            this.TweetModel.Value  = this.TwitterClient.TweetItems.First(t => t.TweetModel.Value.IsSelected)
                                                                  .TweetModel.Value;
            this.SetRetweetButtonForegroundAndText();
            this.SetImagesPageVisibility();
        }

        /// <summary>
        /// リツイート状態を切替える
        /// </summary>
        public async void ChangeIsRetweeted()
        {
            var newIsRetweeted = (bool) !this.TweetModel.Value.TweetStatus.IsRetweeted;
            var statusResponse = await this.TwitterClient.ChangeIsRetweetedAsync(this.TweetModel.Value, newIsRetweeted);

            if (newIsRetweeted)
            {
                this.TweetModel.Value.MyRetweetId = statusResponse.Id;
            }

            this.TweetModel.Value.TweetStatus.IsRetweeted = newIsRetweeted;
            this.SetRetweetButtonForegroundAndText();
        }

        /// <summary>
        /// リツイートボタンの色とテキストを設定する
        /// </summary>
        /// <returns></returns>
        private void SetRetweetButtonForegroundAndText()
        {
            if (this.TweetModel.Value.TweetStatus.IsRetweeted == true)
            {
                this.RetweetButtonForeground.Value = "LawnGreen";
                this.RetweetButtonText.Value      = "リツイートを取り消す";
                return;
            }

            this.RetweetButtonForeground.Value = "White";
            this.RetweetButtonText.Value      = "リツイートする";
        }
        
        /// <summary>
        /// 添付画像に応じて画像レイアウトの Visibility を設定する
        /// </summary>
        private void SetImagesPageVisibility()
        {
            if (this.TweetModel.Value.IsImages1Page)
            {
                this.IsImages1Page.Value = Visibility.Visible;
                return;
            }

            if (this.TweetModel.Value.IsImages2Page)
            {
                this.IsImages2Page.Value = Visibility.Visible;
                return;
            }

            if (this.TweetModel.Value.IsImages3Page)
            {
                this.IsImages3Page.Value = Visibility.Visible;
                return;
            }

            if (this.TweetModel.Value.IsImages4Page)
            {
                this.IsImages4Page.Value = Visibility.Visible;
            }
        }

        /// <summary>
        /// ユーザ画面に遷移する
        /// </summary>
        public void NavigateUserPage()
        {
            this.NavigationService.Navigate("UserItem", null);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            this.Disposable.Dispose();
        }
    }
}
