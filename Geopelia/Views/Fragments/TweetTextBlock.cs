using System;
using System.Globalization;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
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

        private void ParseText()
        {
            if (this.TwitterResponse == null || this.TextBlock == null) return;

            var currTextIdx = 0;
            var stringInfo  = new StringInfo(this.TwitterResponse.TweetStatus.Text);

            this.TextBlock.Inlines.Clear();
            foreach (var url in this.TwitterResponse.TweetStatus.Entities.Urls)
            {
                var begin = url.Indices[0];
                var end   = url.Indices[1];
                if (currTextIdx != begin)
                {
                    var run = new Run { Text = SubStringAndDecode(stringInfo.String, currTextIdx, begin - currTextIdx) };
                    this.TextBlock.Inlines.Add(run);
                }
                var link = new Hyperlink { NavigateUri = new Uri(url.Url) };
                link.Inlines.Add(new Run { Text = url.ExpandedUrl });
                this.TextBlock.Inlines.Add(link);

                currTextIdx = end;
            }

            if (currTextIdx > stringInfo.LengthInTextElements) return;
            this.TextBlock.Inlines.Add(new Run
            {
                Text = SubStringAndDecode(stringInfo.String, currTextIdx, stringInfo.LengthInTextElements - currTextIdx)
            });
        }

        private static string SubStringAndDecode(string text, int start, int length)
        {
            var result = "";
            for (var i = start; i < start + length; i++)
            {
                result += StringInfo.GetNextTextElement(text, i);
            }

            return WebUtility.HtmlDecode(result);
        }
    }
}
