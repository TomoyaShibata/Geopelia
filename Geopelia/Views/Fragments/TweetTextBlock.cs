using System;
using System.Globalization;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using CoreTweet;
using Geopelia.Models;

namespace Geopelia.Views.Fragments
{
    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    public sealed class TweetTextBlock : Control
    {
        private TextBlock TextBlock { get; set; }

        public static readonly DependencyProperty TwitterResponseProperty =
            DependencyProperty.Register(
                nameof(TweetTextBlock.TwitterResponse),
                typeof(TweetModel),
                typeof(TweetTextBlock),
                new PropertyMetadata(null, (sender, args) => { ((TweetTextBlock)sender).ParseText(); }));

        public TweetModel TwitterResponse
        {
            get { return (TweetModel)GetValue(TwitterResponseProperty); }
            set { SetValue(TwitterResponseProperty, value); }
        }

        public TweetTextBlock()
        {
            this.DefaultStyleKey = typeof(TweetTextBlock);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.TextBlock = this.GetTemplateChild("PART_TextBlock") as TextBlock;
            this.ParseText();
        }

        /// <summary>
        /// ツイート本文をパートごとに分割して加工を施します
        /// </summary>
        private void ParseText()
        {
            if (this.TwitterResponse == null || this.TextBlock == null) return;

            var tweetStatus   = this.TwitterResponse.TweetStatus.RetweetedStatus ?? this.TwitterResponse.TweetStatus;
            this.TextBlock.Inlines.Clear();
            foreach (var textPart in tweetStatus.EnumerateTextParts())
            {
                switch (textPart.Type)
                {
                    case TextPartType.Hashtag:
                        var hashtag = new Hyperlink();
                        hashtag.Inlines.Add(new Run { Text = textPart.Text });
                        this.TextBlock.Inlines.Add(hashtag);
                        break;
                    case TextPartType.Plain:
                        this.TextBlock.Inlines.Add(new Run { Text = textPart.Text });
                        break;
                    case TextPartType.Url:
                        var url = new Hyperlink { NavigateUri = new Uri(textPart.RawText) };
                        url.Inlines.Add(new Run { Text =  textPart.Text });
                        this.TextBlock.Inlines.Add(url);
                        break;
                }
            }

        }
    }
}
