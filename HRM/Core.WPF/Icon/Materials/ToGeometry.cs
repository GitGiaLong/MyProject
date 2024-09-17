using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Extensions;
using Core.WPF.Icon.Font.ToGeometries;
using Core.WPF.Icon.Helpers;

namespace Core.WPF.Icon.Materials
{
    public class ToGeometry : ToGeometryBase<MaterialIcons>
    {
        private static readonly Lazy<Typeface> Typeface = new(LoadTypeface);
        public ToGeometry(MaterialIcons icon) : base(icon)
        {
        }

        protected override Typeface TypefaceFor(MaterialIcons icon)
        {
            return Typeface.Value;
        }

        private static Typeface LoadTypeface()
        {
            var typeFace = MaterialDesignFont.Wpf.Value.GetTypefaces().FirstOrDefault();
            if (typeFace == null)
                IconHelper.Throw($"No font / typeface loaded for '{MaterialDesignFont.FontName}'.");
            return typeFace;
        }
    }
}
