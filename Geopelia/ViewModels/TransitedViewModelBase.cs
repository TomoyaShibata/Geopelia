using System.Collections.Generic;
using System.Reactive.Disposables;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Geopelia.ViewModels
{
    public class TransitedViewModelBase : ViewModelBase
    {
        protected INavigationService NavigationService;
        protected TwitterClient      TwitterClient;

        protected CompositeDisposable Disposable = new CompositeDisposable();

        /// <summary>
        /// 前画面に戻ります
        /// </summary>
        public void GoBack() => this.NavigationService.GoBack();

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            this.Disposable.Dispose();
        }
    }
}
