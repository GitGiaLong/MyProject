using Core.Libraries.WPF.Icons.Helpers;

namespace Core.Libraries.WPF.Icons.Font.Sources
{
    [MarkupExtensionReturnType(typeof(ImageSource))]
    public abstract class IconSourceBase<TEnum> : MarkupExtension where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly TEnum Icon;
        protected ImageSource ImageSource;

        protected IconSourceBase(TEnum icon)
        {
            Icon = icon;
            /// ReSharper disable once VirtualMemberCallInConstructor
            UpdateImageSource();
        }

        private Brush _foreground = IconHelper.DefaultBrush;
        public Brush Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground.Equals(value)) return;
                _foreground = value;
                UpdateImageSource();
            }
        }

        private double _size = IconHelper.DefaultSize;
        public double Size
        {
            get => _size;
            set
            {
                if (Math.Abs(_size - value) < 0.5) return;
                _size = value;
                UpdateImageSource();
            }
        }

        protected abstract void UpdateImageSource();
        public override object ProvideValue(IServiceProvider serviceProvider) { return ImageSource; }
    }
}
