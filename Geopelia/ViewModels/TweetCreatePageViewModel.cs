using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Geopelia.Models;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Reactive.Bindings;

namespace Geopelia.ViewModels
{
    public class TweetCreatePageViewModel : ViewModelBase
    {
        public ReactiveProperty<string> TweetText { get; set; } = new ReactiveProperty<string>("");
        public ReadOnlyReactiveCollection<string> FriendScreenNames { get; set; }
        public ReadOnlyReactiveCollection<string> FilteredFriendScreenNames { get; set; }
        public ReactiveProperty<bool> IsFriendScreenNamesShow { get; set; } = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveCollection<BitmapImage> SelectedPictures { get; set; }

        private readonly TwitterClient _twitterClient;
        private readonly PictureModel _pictureModel = new PictureModel();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="iNavigationService"></param>
        /// <param name="twitterClient"></param>
        public TweetCreatePageViewModel(INavigationService iNavigationService, TwitterClient twitterClient)
        {
            this._twitterClient = twitterClient;
            this.FriendScreenNames = this._twitterClient.FriendScreenNames.ToReadOnlyReactiveCollection();
            this.FilteredFriendScreenNames =
                this._twitterClient.FilteredFriendScreenNames.ToReadOnlyReactiveCollection();

            this._twitterClient.SetFriendScreenNames();

            this.SelectedPictures = this._pictureModel.PictureFilePaths.ToReadOnlyReactiveCollection();
        }

        public void CheckInputKey(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var caretIndex = textBox?.SelectionStart;
            if (textBox.Text.Length == 0)
            {
                this.IsFriendScreenNamesShow.Value = false;
                return;
            }

            if (this.IsFriendScreenNamesShow.Value)
            {
                this.FilteringFriendScreenNames(textBox.Text, caretIndex.Value);
            }

            if (textBox.Text[(int)(caretIndex - 1)].ToString() == "@")
            {
                this.IsFriendScreenNamesShow.Value = true;
                return;
            }

            if (textBox.Text[(int)(caretIndex - 1)].ToString() == " ")
            {
                this.IsFriendScreenNamesShow.Value = false;
            }
        }

        /// <summary>
        /// フォロー ScreenName リストをフィルタリングする
        /// </summary>
        /// <param name="inputTweetText">ツイート文字列</param>
        /// <param name="caretIndex">カーソル位置</param>
        private void FilteringFriendScreenNames(string inputTweetText, int caretIndex)
        {
            this._twitterClient.FilteringFriendScreenNames(inputTweetText, caretIndex);
        }

        /// <summary>
        /// Tweet を Post する
        /// </summary>
        public void PostTweet()
        {
            this._twitterClient.PostTweetAsync(this.TweetText.Value, this._pictureModel.PickMultipleFilesAsync);
        }

        /// <summary>
        /// 画像添付のために画像フォルダを参照する
        /// </summary>
        public void OpenPictureLiblary()
        {
            this._pictureModel.PickupPicturesAsync();
        }
    }
}
