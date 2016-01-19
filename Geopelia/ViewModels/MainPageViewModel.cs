using System;
using Prism.Windows.Mvvm;

namespace Geopelia.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public int PivotItemWidth { get; set; } = 400;

        private string _nowDateTime = DateTime.Now.ToString();
        public string NowDateTime
        {
            get { return this._nowDateTime; }
            set { this.SetProperty(ref this._nowDateTime, value); }
        }

        public MainPageViewModel()
        {
        }
    }
}
