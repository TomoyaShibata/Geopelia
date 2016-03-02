using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Data;
using Geopelia.Models;
using Prism.Unity.Windows;
using Microsoft.Practices.Unity;

namespace Geopelia
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    [Bindable]
    sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            this.NavigationService.Navigate("Main", null);
            return Task.CompletedTask;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //this.Container.RegisterType<TokenModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<TwitterClient>(new ContainerControlledLifetimeManager());
        }
    }
}
