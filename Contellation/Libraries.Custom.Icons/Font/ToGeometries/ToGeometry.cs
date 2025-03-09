using Libraries.Custom.Icons.Enums;
using Libraries.Custom.Icons.Font.ToGeometries;
using Libraries.Custom.Icons.Helpers;

namespace Libraries.Custom.Icons
{
    public class ToGeometry : ToGeometryBase<IconType>
    {
        private IconFont _iconFont = IconFont.Auto;

        public ToGeometry(IconType icon) : base(icon) { }

        public IconFont IconFont
        {
            get => _iconFont;
            set
            {
                if (_iconFont.Equals(value)) return;
                _iconFont = value;
                UpdateGeometry();
            }
        }

        protected override Typeface TypefaceFor(IconType icon) { return IconHelper.TypefaceFor(icon, _iconFont); }
    }
}
