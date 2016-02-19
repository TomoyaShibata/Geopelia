using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Windows.Storage.Pickers;
using Prism.Mvvm;

namespace Geopelia.Models
{
    public class PictureModel : BindableBase
    {
        private ObservableCollection<string> _pictureFilePaths = new ObservableCollection<string>();
        public ObservableCollection<string> PictureFilePaths
        {
            get { return this._pictureFilePaths; }
            set { this.SetProperty(ref this._pictureFilePaths, value); }
        }

        /// <summary>
        /// 画像添付のために画像フォルダを参照する
        /// </summary>
        public async void PickupPicturesAsync()
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".gif");

            var pickMultipleFilesAsync = await filePicker.PickMultipleFilesAsync();
            pickMultipleFilesAsync.ToObservable()
                                  .Subscribe(f => this._pictureFilePaths.Add(f.Path));
        }
    }
}
