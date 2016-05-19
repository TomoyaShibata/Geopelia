using System.Collections.Generic;
using Geopelia.Models;
using Prism.Windows.Navigation;

namespace Geopelia.ViewModels
{
    public class TweetDetailsPageViewModel : TransitedViewModelBase
    {

        public TweetDetailsPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}
