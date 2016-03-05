using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Geopelia.ViewModels
{
    public class IndexPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }

        public IndexPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }
    }
}
