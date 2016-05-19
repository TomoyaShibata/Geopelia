using System.Collections.Generic;
using System.Linq;
using Geopelia.Models;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetDetailsPageViewModel : TransitedViewModelBase
    {
        public ReactiveProperty<TweetModel> TweetModel = new ReactiveProperty<TweetModel>();

        public TweetDetailsPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this.NavigationService = navigationService;
            this.TwitterClient     = twitterClient;
            this.TweetModel.Value  = this.TwitterClient.TweetItems.First(t => t.TweetModel.Value.IsSelected)
                                                                  .TweetModel.Value;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            this.TweetModel.Value.IsSelected = false;
            this.Disposable.Dispose();
        }
    }
}
