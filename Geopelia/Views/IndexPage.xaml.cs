using Windows.UI.Xaml.Controls;
using Geopelia.ViewModels;

namespace Geopelia.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        private IndexPageViewModel ViewModel => this.DataContext as IndexPageViewModel;

        public IndexPage()
        {
            this.InitializeComponent();
        }
    }
}
