using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class TweetCreatePage
    {
        private TweetCreatePageViewModel ViewModel => this.DataContext as TweetCreatePageViewModel;

        public TweetCreatePage()
        {
            this.InitializeComponent();
        }
    }
}
