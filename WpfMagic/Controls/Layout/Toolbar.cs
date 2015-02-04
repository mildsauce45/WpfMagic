using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfMagic.Contracts;

namespace WpfMagic.Controls.Layout
{
    public class Toolbar : Panel, IContentAreaProvider
    {
        #region Spacer Dependency Property

        public static DependencyProperty SpacerProperty =
            DependencyProperty.Register("Spacer", typeof(double), typeof(Toolbar), new PropertyMetadata(10d));

        public double Spacer
        {
            get { return (double)GetValue(SpacerProperty); }
            set { SetValue(SpacerProperty, value); }
        }

        #endregion

        #region Orientation Dependency Property

        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Toolbar), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region IContentAreaProvider Members

        public string ContentArea { get; set; }

        #endregion

        #region Measure/Arrange Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            var height = 0d;
            var width = 0d;
            var children = 0;

            foreach (var uie in this.Children.OfType<UIElement>())
            {
                if (uie.Visibility == Visibility.Collapsed)
                    continue;

                uie.Measure(availableSize);

                var desiredSize = uie.DesiredSize;

                if (Orientation == Orientation.Horizontal)
                {
                    height = Math.Max(desiredSize.Height, height);
                    width += desiredSize.Width;
                }
                else
                {
                    width = Math.Max(desiredSize.Width, width);
                    height += desiredSize.Height;
                }

                children++;
            }

            if (children > 0)
            {
                width += (Orientation == Orientation.Horizontal) ? (children - 1) * Spacer : 0;
                height += (Orientation == Orientation.Vertical) ? (children - 1) * Spacer : 0;
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var x = 0d;
            var y = 0d;
            
            var visibleChildren = this.Children.OfType<UIElement>().Where(uie => uie.Visibility == Visibility.Visible).ToList();            

            foreach (var uie in visibleChildren)
            {
                uie.Arrange(new Rect(x, y, uie.DesiredSize.Width, uie.DesiredSize.Height));

                if (Orientation == Orientation.Horizontal)
                {
                    x += uie.DesiredSize.Width;

                    var index = visibleChildren.IndexOf(uie);

                    if (index < visibleChildren.Count - 1)
                        x += Spacer;
                }
                else
                {
                    y += uie.DesiredSize.Height;

                    var index = visibleChildren.IndexOf(uie);

                    if (index < visibleChildren.Count - 1)
                        y += Spacer;
                }
            }

            return finalSize;
        }

        #endregion
    }
}
