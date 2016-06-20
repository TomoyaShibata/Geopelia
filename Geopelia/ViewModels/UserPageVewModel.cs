using System.Collections.Generic;
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
        public ReadOnlyReactiveCollection<TweetItemViewModel> UserTweetItems { get; set; }
        public ReadOnlyReactiveCollection<User> Users { get; set; }
        public ReactiveProperty<UserItem> User { get; set; } = new ReactiveProperty<UserItem>();

        private long UserId { get; set; }

        public UserPageViewModel(INavigationService navigationService, TwitterClient twitterClient)
        {
            this.NavigationService = navigationService;
            this.TwitterClient     = twitterClient;
            this.Users             = this.TwitterClient.Followers.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            this.UserTweetItems    = this.TwitterClient.UserTweetItems.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            this.UserId      = (long)e.Parameter;
            this.User.Value  = new UserItem(await this.TwitterClient.GetUser(this.UserId));
            this.TwitterClient.RefreshUserTweetItems(this.NavigationService, this.UserId);
        }

        public async void LoadPastUserTweetItems()
        {
            await this.TwitterClient.LoadPastUserTweetItems(this.NavigationService, this.UserId);
        }
    }
}
