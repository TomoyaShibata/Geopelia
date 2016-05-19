using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class MainPage
    {
        private MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
