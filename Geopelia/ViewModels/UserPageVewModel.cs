using CoreTweet;
using Geopelia.Models;
using Prism.Windows.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Geopelia.ViewModels
{
    public class UserPageViewModel : TransitedViewModelBase
    {
        public ReadOnlyReactiveCollection<TweetModel> Timelines { get; set; }
        public ReadOnlyReactiveCollection<User>       Users { get; set; } 

        public UserPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this.NavigationService = navigationService;
            this.TwitterClient     = twitterClient;

            this.Timelines = this.TwitterClient.Timelines.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            this.Users     = this.TwitterClient.Followers.ToReadOnlyReactiveCollection().AddTo(this.Disposable);

            this.TwitterClient.GetMyFollowers();
        }
    }
}
