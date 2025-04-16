using Libraries.Custom.Enums.Icon;
using Libraries.Custom.Helpers.Icon;
using Libraries.Custom.Icons.Font.Sources;
using Libraries.Custom.Interfaces.Icon;

namespace Libraries.Custom.Icons
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
