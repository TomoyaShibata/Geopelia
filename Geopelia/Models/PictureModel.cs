using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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

        public IReadOnlyList<StorageFile> PickMultipleFilesAsync { get; set; }
        public List<IRandomAccessStream> Pic { get; set; } = new List<IRandomAccessStream>();


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

            this.PickMultipleFilesAsync = await filePicker.PickMultipleFilesAsync();
            this.PickMultipleFilesAsync?.ToObservable().Subscribe(async f =>
            {
                var stream      = await f.OpenAsync(Windows.Storage.FileAccessMode.Read);
                this.Pic.Add(stream);
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                this._pictureFilePaths.Add(bitmapImage);
            });
        }
    }
}
