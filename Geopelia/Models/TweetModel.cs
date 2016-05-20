using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CoreTweet;
using CoreTweet.Streaming;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Geopelia.Models
{
    /// <summary>
    /// ツイート Model
    /// </summary>
    public class TweetModel : BindableBase
    {
        /// <summary>
        /// ツイート ID
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
        /// プロフィール画像 URL
        /// </summary>
        public string ProfileImageUrlHttps { get; set; }
        /// <summary>
        /// RT 名前
        /// </summary>
        public string RtName { get; set; }
        /// <summary>
        /// RT スクリーンネーム
        /// </summary>
        public string RtScreenName { get; set; }
        /// <summary>
        /// RT プロフィール画像 URL
        /// </summary>
        public string RtProfileImageUrlHttps { get; set; } = "";
        /// <summary>
        /// 返信先ツイート ID
        /// </summary>
        public long? InReplyToStatusId { get; set; }
        /// <summary>
        /// 返信先ツイート ID
        /// </summary>
        public TweetModel ReplyTweet { get; set; }

        /// <summary>
        /// ボーダーカラー
        /// </summary>
        public SolidColorBrush BorderColor { get; set; }

        /// <summary>
        /// ツイート先ステータス
        /// </summary>
        public Status RetweetedStatus { get; set; }

        /// <summary>
        /// 添付画像 URI リスト
        /// </summary>
        public List<string> PicTwitterUris { get; set; }

        /// <summary>
        /// ツイート
        /// </summary>
        public Status TweetStatus { get; set; }

        /// <summary>
        /// リプライ先ツイート
        /// </summary>
        public StatusResponse ReplyStatusMessage { get; set; }

        /// <summary>
        /// 自分がリツイートしたときに生成されたツイートのID
        /// </summary>
        public long MyRetweetId { get; set; }

        /// <summary>
        /// Twitter クライアント名
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 自分が選択されたツイートかどうか
        /// </summary>
        public bool IsSelected { get; set; } = false;

        public bool IsImages1Page { get; set; } = false;
        public bool IsImages2Page { get; set; } = false;
        public bool IsImages3Page { get; set; } = false;
        public bool IsImages4Page { get; set; } = false;

        public TweetModel(StatusMessage statusMessage)
        {
            this.InitTweetModel(statusMessage.Status);
        }

        public TweetModel(Status status)
        {
            this.InitTweetModel(status);
        }

        private void InitTweetModel(Status s)
        {
            this.TweetStatus = s;
            this.SetPicTwitterUris(s);

            this.RetweetedStatus = s.RetweetedStatus;
            if (s.RetweetedStatus != null)
            {
                this.SetRetweet(s);
                return;
            }

            this.CreatedAt            = s.CreatedAt.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            this.Id                   = s.Id;
            this.Text                 = s.Text;
            this.Name                 = s.User.Name;
            this.ScreenName           = s.User.ScreenName;
            this.ProfileImageUrlHttps = s.User.ProfileImageUrlHttps;
            this.ClientName           = s.ParseSource().Name;
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
        private void SetRetweet(Status s)
        {
            this.CreatedAt              = s.RetweetedStatus.CreatedAt.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Id                     = s.RetweetedStatus.Id;
            this.Text                   = s.RetweetedStatus.Text;
            this.Name                   = s.RetweetedStatus.User.Name;
            this.ScreenName             = s.RetweetedStatus.User.ScreenName;
            this.RtName                 = s.User.Name + "さんがリツイート";
            this.RtScreenName           = s.User.ScreenName;
            this.ProfileImageUrlHttps   = s.RetweetedStatus.User.ProfileImageUrlHttps;
            this.RtProfileImageUrlHttps = s.User.ProfileImageUrlHttps;
            this.ClientName             = s.ParseSource().Name;
        }

        /// <summary>
        /// 添付画像の URI をコレクションに格納する
        /// <param name="status">status</param>
        /// </summary>
        private void SetPicTwitterUris(Status status)
        {
            if (status.Entities.Media == null) return;
            this.PicTwitterUris = new List<string>();
            status.ExtendedEntities.Media.ForEach(m => this.PicTwitterUris.Add(m.MediaUrlHttps));

            switch (this.PicTwitterUris.Count)
            {
                case 1:
                    this.IsImages1Page = true;
                    break;
                case 2:
                    this.IsImages2Page = true;
                    break;
                case 3:
                    this.IsImages3Page = true;
                    break;
                case 4:
                    this.IsImages4Page = true;
                    break;
            }
        }
    }
}
