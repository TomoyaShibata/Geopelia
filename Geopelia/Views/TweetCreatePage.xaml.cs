using Windows.UI.Xaml.Controls;
using Geopelia.ViewModels;

namespace Geopelia.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class TweetCreatePage : Page
    {
        private TweetCreatePageViewModel ViewModel => this.DataContext as TweetCreatePageViewModel;

        public TweetCreatePage()
        {
            this.InitializeComponent();
        }
    }
}
