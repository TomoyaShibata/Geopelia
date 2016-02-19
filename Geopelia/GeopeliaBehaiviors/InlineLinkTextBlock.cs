using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Microsoft.Xaml.Interactivity;

namespace Geopelia.GeopeliaBehaiviors
{
    public class InlineLinkTextBlock : Behavior<TextBlock>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += TextChangedEventHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }


        public void TextChangedEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            var textBlock = sender as TextBlock;
            var run       = new Run {Text = "クラりん可愛い"};
            var hyperlink = new Hyperlink();
            hyperlink.NavigateUri = new Uri("https://www.google.co.jp/");
            hyperlink.Inlines.Add(run);


            textBlock.Inlines.Add(hyperlink);

        }
    }
}
