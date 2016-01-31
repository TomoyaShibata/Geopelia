using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CoreTweet;
using CoreTweet.Streaming;

namespace Geopelia.Models
{
    public class TweetModel
    {
        /// <summary>
        /// ツイートID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// ツイート作成日時
        /// </summary>
        public string CreatedAt { get; set; }
        /// <summary>
        /// ツイート本文
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// スクリーンネーム
        /// </summary>
        public string ScreenName { get; set; }
        /// <summary>
        /// プロフィール画像URL
        /// </summary>
        public Uri ProfileImageUrlHttps { get; set; }
        /// <summary>
        /// RT 名前
        /// </summary>
        public string RtName { get; set; }
        /// <summary>
        /// RT スクリーンネーム
        /// </summary>
        public string RtScreenName { get; set; }
        /// <summary>
        /// RT プロフィール画像URL
        /// </summary>
        public Uri RtProfileImageUrlHttps { get; set; }
        /// <summary>
        /// 返信先ツイートID
        /// </summary>
        public long? InReplyToStatusId { get; set; }
        /// <summary>
        /// 返信先ツイートID
        /// </summary>
        public TweetModel ReplyTweet { get; set; }

        /// <summary>
        /// ボーダーカラー
        /// </summary>
        public SolidColorBrush BorderColor { get; set; }

        /// <summary>
        /// 返信先ツイートID
        /// </summary>
        public Status RetweetedStatus { get; set; }

        public StatusMessage TweetStatusMessage { get; set; }

        public TweetModel(StatusMessage s)
        {
            TweetStatusMessage = s;
		
            //this.BorderColor = this.SetBorderBrushColor(s);
            this.RetweetedStatus      = s.Status.RetweetedStatus;
            if (s.Status.RetweetedStatus != null)
            {
                this.SetRetweet(s);
                return;
            }

            this.CreatedAt            = s.Status.CreatedAt.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            this.Id                   = s.Status.Id;
            this.Text                 = s.Status.Text;
            this.Name                 = s.Status.User.Name;
            this.ScreenName           = s.Status.User.ScreenName;
            this.ProfileImageUrlHttps = new Uri(s.Status.User.ProfileImageUrlHttps);
        }

        private SolidColorBrush SetBorderBrushColor(StatusMessage s)
        {
            if (s.Status.RetweetedStatus != null)
            {
                return new SolidColorBrush(Colors.LimeGreen);
            }

            if (s.Status.Text.Contains("@tomoya_shibata"))
            {
                return new SolidColorBrush(Colors.Pink);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        /// リツイート情報をセットする
        /// </summary>
        /// <param name="s"></param>
        private void SetRetweet(StatusMessage s)
        {
            this.CreatedAt              = s.Status.RetweetedStatus.CreatedAt.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Id                     = s.Status.RetweetedStatus.Id;
            this.Text                   = s.Status.RetweetedStatus.Text;
            this.Name                   = s.Status.RetweetedStatus.User.Name;
            this.ScreenName             = s.Status.RetweetedStatus.User.ScreenName;
            this.RtName                 = s.Status.User.Name + "さんがリツイート";
            this.RtScreenName           = s.Status.User.ScreenName;
            this.ProfileImageUrlHttps   = new Uri(s.Status.User.ProfileImageUrlHttps);
            this.RtProfileImageUrlHttps = new Uri(s.Status.RetweetedStatus.User.ProfileImageUrlHttps);
        }
    }
}
