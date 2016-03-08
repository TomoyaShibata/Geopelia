using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Geopelia.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }

        public SettingPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        /// <summary>
        /// 認証情報、アプリ設定を全て削除する
        /// </summary>
        public void ResetSetting()
        {
            var tokenModel = new TokenModel();
            tokenModel.RemoveAllTokens();
        }
    }
}
