using Windows.UI.Xaml.Controls;
using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class UserPage : Page
    {
        private UserPageViewModel ViewModel => this.DataContext as UserPageViewModel;

        public UserPage()
        {
            this.InitializeComponent();
        }
    }
}
