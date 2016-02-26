using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Mvvm;

namespace Geopelia.Models
{
    public class PictureModel : BindableBase
    {
        private ObservableCollection<BitmapImage> _pictureFilePaths = new ObservableCollection<BitmapImage>();
        public ObservableCollection<BitmapImage> PictureFilePaths
        {
            get { return this._pictureFilePaths; }
            set { this.SetProperty(ref this._pictureFilePaths, value); }
        }

        public IReadOnlyList<StorageFile> PickedPictures { get; set; }

        /// <summary>
        /// ツイートに添付する画像選択画面を開く<br></br>
        /// 選択された画像は画面描画用の BitmapImage と、内部で確保するための StorageFile にそれぞれ保存される
        /// </summary>
        public async void PickPicturesAsync()
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode               = PickerViewMode.Thumbnail
            };

            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".gif");

            this.PickedPictures = await filePicker.PickMultipleFilesAsync();

            this.PickedPictures?.ToObservable().Subscribe(async f =>
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(await f.OpenAsync(FileAccessMode.Read));
                this._pictureFilePaths.Add(bitmapImage);
            });
        }
    }
}
