using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Font.Sources;
using Core.Libraries.WPF.Icons.Helpers;
using Core.Libraries.WPF.Icons.Interfaces;

namespace Core.Libraries.WPF.Icons.Font
{
    public class IconSource : IconSourceBase<IconType>, IIconFont
    {
        private IconFont _iconFont = IconFont.Auto;

        public IconSource(IconType icon) : base(icon) { }

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
        protected override void UpdateImageSource()
        {
            ImageSource = Icon.ToImageSource(IconFont, Foreground, Size);
            ///Pro
            //ImageSource = Icon.WpfFontFor(_iconFont).ToImageSource(Icon, Foreground, Size);
        }
    }
}
