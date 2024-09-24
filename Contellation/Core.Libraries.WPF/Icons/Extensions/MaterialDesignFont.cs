using Core.Libraries.WPF.Icons.Helpers;
using System.Reflection;

namespace Core.Libraries.WPF.Icons.Extensions
{
    using WinFormsFont = System.Drawing.FontFamily;
    using WpfFont = System.Windows.Media.FontFamily;
    internal class MaterialDesignFont
    {
        public const string FontName = "Material Design Icons";
        public static readonly Lazy<WinFormsFont> WinForms = new(LoadWinFormsFont);
        public static readonly Lazy<WpfFont> Wpf = new(LoadWpfFont);

        private static readonly Assembly FontAssembly = typeof(MaterialDesignFont).Assembly;
        private static WpfFont LoadWpfFont()
        {
            return FontAssembly.LoadFont("Fonts", FontName);
        }

        private static WinFormsFont LoadWinFormsFont()
        {
            return FontAssembly.LoadResourceFont("Fonts", "materialdesignicons-webfont.ttf");
        }
    }
}
