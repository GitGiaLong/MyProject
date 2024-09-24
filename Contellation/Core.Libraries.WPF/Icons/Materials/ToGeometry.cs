using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Extensions;
using Core.Libraries.WPF.Icons.Font.ToGeometries;
using Core.Libraries.WPF.Icons.Helpers;

namespace Core.Libraries.WPF.Icons.Materials
{
    public class ToGeometry : ToGeometryBase<MaterialIcons>
    {
        private static readonly Lazy<Typeface> Typeface = new(LoadTypeface);
        public ToGeometry(MaterialIcons icon) : base(icon) { }

        protected override Typeface TypefaceFor(MaterialIcons icon) { return Typeface.Value; }

        private static Typeface LoadTypeface()
        {
            var typeFace = MaterialDesignFont.Wpf.Value.GetTypefaces().FirstOrDefault();
            if (typeFace == null) { IconHelper.Throw($"No font / typeface loaded for '{MaterialDesignFont.FontName}'."); }
            return typeFace;
        }
    }
}
