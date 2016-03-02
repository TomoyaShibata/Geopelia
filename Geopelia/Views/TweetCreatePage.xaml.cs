using Windows.UI.Xaml.Controls;
using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class TweetCreatePage : Page
    {
        private TweetCreatePageViewModel ViewModel => this.DataContext as TweetCreatePageViewModel;

        public TweetCreatePage()
        {
            this.InitializeComponent();
        }
    }
}
