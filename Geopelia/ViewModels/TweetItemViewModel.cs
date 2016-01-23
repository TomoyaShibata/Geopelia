using Windows.UI.Xaml.Media;
using CoreTweet.Streaming;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetItemViewModel : ViewModelBase
    {
        private readonly INavigationService _iNavigationService;
        private readonly TwitterClient _tweetClient;

        private ReactiveProperty<TweetModel> _tweetModel = new ReactiveProperty<TweetModel>();

        public ReactiveProperty<TweetModel> TweetModel
        {
            get { return this._tweetModel; }
            set { this.SetProperty(ref this._tweetModel, value); }
        }

        public ReactiveProperty<Brush> BorderBrush = new ReactiveProperty<Brush>();
        public ReactiveProperty<string> Brush = new ReactiveProperty<string>();

        public TweetItemViewModel(INavigationService iNavigationService, StatusMessage s, TwitterClient twitterClient)
        {
            this._iNavigationService = iNavigationService;
            this._tweetClient = twitterClient;
            this._tweetModel.Value = new TweetModel(s);
            Brush.Value = this.SetBorderBrushColor();
        }

        public void TappedEventHandler()
        {

        }

        public void Retweet()
        {
            this._tweetClient.Retweet(this.TweetModel.Value.Id);
        }

        public void Favorite()
        {
            this._tweetClient.Favorite(this.TweetModel.Value.Id);
        }

        public void NavigateTweetCreatePage()
        {
            this._iNavigationService.Navigate("TweetCreate", this.TweetModel.Value.Id);
        }

        private string SetBorderBrushColor()
        {
            if (this.TweetModel.Value.RetweetedStatus != null)
            {
                return "DodgerBlue";
            }

            if (this.TweetModel.Value.Text.Contains("@tomoya_shibata"))
            {
                return "LimeGreen";
            }

            return "Transparent";
        }
    }
}