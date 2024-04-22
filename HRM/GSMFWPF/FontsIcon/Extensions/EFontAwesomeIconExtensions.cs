#if WINDOWS_UWP
using Windows.UI.Xaml.Media;
#elif WINUI
using Microsoft.UI.Xaml.Media;
#else
using System.Windows.Media;
#endif

namespace GSMFWPF.FontsIcon.Extensions
{
    public static class EFontAwesomeIconExtensions
    {
#if !WINDOWS_UWP && !WINUI
        /// <summary>
        /// Get the Typeface of an icon
        /// </summary>
        public static Typeface GetTypeFace(this EFontAwesomeIcon icon)
        {
            switch (icon.GetStyle())
            {
                case EFontAwesomeStyle.Regular: return Fonts.RegularTypeface;
                case EFontAwesomeStyle.Solid: return Fonts.SolidTypeface;
                case EFontAwesomeStyle.Brands: return Fonts.BrandsTypeface;
            }

            return Fonts.RegularTypeface;
        }
#endif
        /// <summary>
        /// Get the FontFamily of an icon
        /// </summary>
        public static FontFamily GetFontFamily(this EFontAwesomeIcon icon)
        {
            switch (icon.GetStyle())
            {
                case EFontAwesomeStyle.Regular: return Fonts.RegularFontFamily;
                case EFontAwesomeStyle.Solid: return Fonts.SolidFontFamily;
                case EFontAwesomeStyle.Brands: return Fonts.BrandsFontFamily;
            }

            return Fonts.RegularFontFamily;
        }

        /// <summary>
        /// Get the Font Awesome information of an icon
        /// </summary>
        public static FontAwesomeInformation GetInformation(this EFontAwesomeIcon icon)
        {
            return FontAwesomeInternal.Information.TryGetValue(icon, out var info) ? info : null;
        }

        /// <summary>
        /// Get the Font Awesome label of an icon
        /// </summary>
        public static string GetLabel(this EFontAwesomeIcon icon)
        {
            return FontAwesomeInternal.Information.TryGetValue(icon, out var info) ? info.Label : null;
        }

        /// <summary>
        /// Get the Font Awesome Style of an icon
        /// </summary>
        public static EFontAwesomeStyle GetStyle(this EFontAwesomeIcon icon)
        {
            return FontAwesomeInternal.Information.TryGetValue(icon, out var info) ? info.Style : EFontAwesomeStyle.None;
        }

        /// <summary>
        /// Get the SVG path of an icon
        /// </summary>
        public static bool GetSvg(this EFontAwesomeIcon icon, out string path, out int width, out int height)
        {
            path = string.Empty;
            width = -1;
            height = -1;
            if (FontAwesomeInternal.Information.TryGetValue(icon, out var info) && info.Svg != null)
            {
                path = info.Svg.Path;
                width = info.Svg.Width;
                height = info.Svg.Height;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the Unicode of an icon
        /// </summary>
        public static string GetUnicode(this EFontAwesomeIcon icon)
        {
            return FontAwesomeInternal.Information.TryGetValue(icon, out var info) ? info.Unicode : char.ConvertFromUtf32(0);
        }


    }

}
