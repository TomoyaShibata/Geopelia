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
            this.SetImagesPageVisibility();
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            this.Disposable.Dispose();
        }

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

        public void NavigateUserPage()
        {
            this.NavigationService.Navigate("User", null);
        }
    }
}
