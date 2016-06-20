using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class HashtagTweetsPage    {        private HashtagTweetsPageViewModel ViewModel => this.DataContext as HashtagTweetsPageViewModel;

        public HashtagTweetsPage()
        {
            this.InitializeComponent();
        }
    }
}
