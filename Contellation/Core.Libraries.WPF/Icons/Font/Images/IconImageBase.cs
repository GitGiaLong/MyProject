using Core.Libraries.WPF.Icons.Helpers;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Icons.Font.Images
{
    public abstract class IconImageBase<TEnum> : Image where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        public TEnum Icon
        {
            get { return (TEnum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(TEnum), typeof(IconImageBase<TEnum>),
            new PropertyMetadata(default(TEnum), OnIconPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is IconImageBase<TEnum> iconImage)) return;
            var icon = (TEnum)e.NewValue;
            var imageSource = iconImage.ImageSourceFor(icon);
            iconImage.SetValue(SourceProperty, imageSource);
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(IconImageBase<TEnum>),
            new PropertyMetadata(IconHelper.DefaultBrush, OnForegroundPropertyChanged));

        private static void OnForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is IconImageBase<TEnum> iconImage)) return;
            var icon = (TEnum)d.GetValue(IconProperty);
            var imageSource = iconImage.ImageSourceFor(icon);
            iconImage.SetValue(SourceProperty, imageSource);
        }

        protected abstract ImageSource ImageSourceFor(TEnum icon);

        protected IconImageBase() { if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum."); }




    }
}
