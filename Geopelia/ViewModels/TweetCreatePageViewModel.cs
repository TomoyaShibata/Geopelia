using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetCreatePageViewModel : ViewModelBase
    {
        public ReactiveProperty<string> TweetText { get; set; } = new ReactiveProperty<string>("");
        private readonly TwitterClient _twitterClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="iNavigationService"></param>
        /// <param name="twitterClient"></param>
        public TweetCreatePageViewModel(INavigationService iNavigationService, TwitterClient twitterClient)
        {
            this._twitterClient = twitterClient;
        }

        /// <summary>
        /// Tweet を Post する
        /// </summary>
        public void PostTweet()
        {
            this._twitterClient.PostTweet(this.TweetText.Value);
        }
    }
}
