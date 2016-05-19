using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class TweetDetailsPage
    {
        private TweetDetailsPageViewModel ViewModel => this.DataContext as TweetDetailsPageViewModel;

        public TweetDetailsPage()
        {
            this.InitializeComponent();
        }
    }
}
