using Windows.UI.Xaml.Controls;
using Geopelia.ViewModels;

namespace Geopelia.Views
{
    public sealed partial class SettingPage : Page
    {
        private SettingPageViewModel ViewModel => this.DataContext as SettingPageViewModel;

        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
