using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Font.Interfaces;
using Core.WPF.Icon.Font.Sources;
using Core.WPF.Icon.Helpers;

namespace Core.WPF.Icon.Font
{
    public class IconSource : IconSourceBase<IconChar>, IHaveIconFont
    {
        private IconFont _iconFont = IconFont.Auto;

        public IconSource(IconChar icon) : base(icon) { }

        protected override void UpdateImageSource()
        {
            ImageSource = Icon.ToImageSource(IconFont, Foreground, Size);
            //ImageSource = Icon.WpfFontFor(_iconFont).ToImageSource(Icon, Foreground, Size);
        }

        public IconFont IconFont
        {
            get => _iconFont;
            set
            {
                if (_iconFont.Equals(value)) return;
                _iconFont = value;
                UpdateImageSource();
            }
        }
    }
}
