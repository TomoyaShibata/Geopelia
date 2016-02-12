using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CoreTweet;
using CoreTweet.Streaming;
using Microsoft.Practices.ObjectBuilder2;

namespace Geopelia.Models
{
    /// <summary>
    /// ツイート Model
    /// </summary>
    public class TweetModel
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
        /// RT プロフィール画像 URL
        /// </summary>
        public Uri RtProfileImageUrlHttps { get; set; }
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

        public TweetModel(StatusMessage s)
        {
            this.TweetStatus = s.Status;

            if (s.Status.Entities.Media != null)
            {
                this.PicTwitterUris = new List<string>();
                s.Status.ExtendedEntities.Media.ForEach(m => this.PicTwitterUris.Add(m.MediaUrlHttps));
            }

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
