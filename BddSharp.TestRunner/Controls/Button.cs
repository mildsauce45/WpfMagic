using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BddSharp.TestRunner.Controls
{
    public class Button : System.Windows.Controls.Button
    {
        #region Dependency Properties

        public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(Button), new PropertyMetadata(null));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(Button), new PropertyMetadata(null));

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

		public static readonly DependencyProperty HoverBorderBrushProperty =
			DependencyProperty.Register("HoverBorderBrush", typeof(Brush), typeof(Button), new PropertyMetadata(null));

		public Brush HoverBorderBrush
		{
			get { return (Brush)GetValue(HoverBorderBrushProperty); }
			set { SetValue(HoverBorderBrushProperty, value); }
		}

        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(Button), new PropertyMetadata(null));

        public Brush HoverForeground
        {
            get { return (Brush)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }

        public static readonly DependencyProperty HoverEffectProperty =
            DependencyProperty.Register("HoverEffect", typeof(Effect), typeof(Button), new PropertyMetadata(null));

        public Effect HoverEffect
        {
            get { return (Effect)GetValue(HoverEffectProperty); }
            set { SetValue(HoverEffectProperty, value); }
        }

		#endregion

        #region Static Contructors

        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        }

        #endregion
    }
}
