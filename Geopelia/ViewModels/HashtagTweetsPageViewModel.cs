using CoreTweet;
using Geopelia.Models;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class HashtagTweetsPageViewModel : TransitedViewModelBase
    {
        public ReadOnlyReactiveCollection<TweetModel> Timelines { get; set; }
        public ReadOnlyReactiveCollection<User>       Users { get; set; } 

        public HashtagTweetsPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this.NavigationService = navigationService;
            this.TwitterClient     = twitterClient;
        }
    }
}
