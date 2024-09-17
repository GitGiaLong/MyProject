using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Extensions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Reflection;

namespace Core.WPF.Icon.Helpers
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class FormsIconHelper
    {
        #region Public

        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        public static bool ThrowOnNullFonts { get; set; } = true;

        /// <summary>
        /// Returns a bitmap for the specified font and icon
        /// </summary>
        /// <typeparam Name="TEnum">icon enum Type (for custom fonts)</typeparam>
        /// <param Name="fontFamily">The icon font</param>
        /// <param Name="icon">The icon</param>
        /// <param Name="width">Width of destination bitmap in pixels</param>
        /// <param Name="height">Height of destination bitmap in pixels</param>
        /// <param Name="color">Icon color</param>
        /// <param Name="rotation">Icon rotation in degrees</param>
        /// <param Name="flip">Icon flip</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap ToBitmap<TEnum>(this System.Drawing.FontFamily fontFamily, TEnum icon,
            int width, int height, System.Drawing.Color? color = null,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var text = icon.ToChar();
                var font = graphics.GetAdjustedIconFont(fontFamily, text, new SizeF(width, height));
                graphics.Rotate(rotation, width, height);
                var brush = color.HasValue ? new SolidBrush(color.Value) : DefaultBrush;
                DrawIcon(graphics, font, text, width, height, brush);
            }

            bitmap.Flip(flip);
            return bitmap;
        }

        /// <summary>
        /// Returns a bitmap for the specified font and icon
        /// </summary>
        /// <typeparam Name="TEnum">icon enum Type (for custom fonts)</typeparam>
        /// <param Name="fontFamily">The icon font</param>
        /// <param Name="icon">The icon</param>
        /// <param Name="size">Size of destination bitmap in pixels</param>
        /// <param Name="color">Icon color</param>
        /// <param Name="rotation">Icon rotation in degrees</param>
        /// <param Name="flip">Icon flip</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap ToBitmap<TEnum>(this System.Drawing.FontFamily fontFamily, TEnum icon,
            int size = DefaultSize, System.Drawing.Color? color = null,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return ToBitmap(fontFamily, icon, size, size, color, rotation, flip);
        }

        /// <summary>
        /// Returns a bitmap for the specified font awesome style and icon
        /// </summary>
        /// <param Name="icon">The icon</param>
        /// <param Name="iconFont">The font awesome style / font to use</param>
        /// <param Name="size">Size of destination bitmap in pixels</param>
        /// <param Name="color">Icon color</param>
        /// <param Name="rotation">Icon rotation in degrees</param>
        /// <param Name="flip">Icon flip</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap ToBitmap(this IconChar icon, IconFont iconFont = IconFont.Auto,
            int size = DefaultSize, System.Drawing.Color? color = null,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = icon.FontFamilyFor(iconFont);
            return fontFamily.ToBitmap(icon, size, color, rotation, flip);
        }

        /// <summary>
        /// Renders an icon to a bitmap image using GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for small icons.
        /// </summary>
        /// <param Name="icon">The icon</param>
        /// <param Name="size">Size of destination bitmap in pixels</param>
        /// <param Name="color">Icon color</param>
        /// <param Name="rotation">Icon rotation in degrees</param>
        /// <param Name="flip">Icon flip</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap ToBitmap(this IconChar icon, System.Drawing.Color? color = null,
            int size = DefaultSize, double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = FontFamilyFor(icon);
            return fontFamily.ToBitmap(icon, size, size, color, rotation, flip);
        }

        /// <summary>
        /// Renders an icon to a bitmap image using GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for small icons.
        /// </summary>
        /// <param Name="icon">The icon</param>
        /// <param Name="width">Width of destination bitmap in pixels</param>
        /// <param Name="height">Height of destination bitmap in pixels</param>
        /// <param Name="color">Icon color</param>
        /// <param Name="rotation">Icon rotation in degrees</param>
        /// <param Name="flip">Icon flip</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap ToBitmap(this IconChar icon,
            int width = DefaultSize, int height = DefaultSize, System.Drawing.Color? color = null,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = FontFamilyFor(icon);
            return fontFamily.ToBitmap(icon, width, height, color, rotation, flip);
        }

        /// <summary>
        /// Renders a text centered to the specified graphics.
        /// </summary>
        /// <param Name="graphics">The graphics to draw the icon text into</param>
        /// <param Name="text">The text to render</param>
        /// <param Name="width">Width of graphics in pixels</param>
        /// <param Name="height">Height of graphics in pixels</param>
        /// <param Name="font">The font to use</param>
        /// <param Name="brush">The color brush to use</param>
        public static void DrawIcon(this Graphics graphics, System.Drawing.Font font, string text,
            int width = DefaultSize, int height = DefaultSize, System.Drawing.Brush brush = null)
        {
            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PageUnit = GraphicsUnit.Pixel;

            var topLeft = graphics.GetTopLeft(text, font, new SizeF(width, height));

            graphics.DrawString(text, font, brush ?? DefaultBrush, topLeft);
        }

        /// <summary>
        /// Shortcut helper method to quickly add a rendered icon to the specified image list
        /// </summary>
        /// <param Name="imageList">The image list to add to</param>
        /// <param Name="icon">The icon to render and add</param>
        /// <param Name="color">The icon color</param>
        /// <param Name="size">The icon size in pixels</param>
        //public static void AddIcon(this ImageList imageList, IconChar icon,
        //    Color? color = null, int size = IconHelper.DefaultSize)
        //{
        //    imageList.Images.Add(icon.ToString(), icon.ToBitmap(color, size));
        //}

        /// <summary>
        /// Shortcut helper method to quickly add rendered icons to the specified image list
        /// </summary>
        /// <param Name="imageList">The image list to add to</param>
        /// <param Name="color">The icon color</param>
        /// <param Name="size">The icon size in pixels</param>
        /// <param Name="icons">The icons to render and add</param>
        //public static void AddIcons(this ImageList imageList,
        //    Color? color = null, int size = IconHelper.DefaultSize, params IconChar[] icons)
        //{
        //    foreach (var icon in icons)
        //        imageList.AddIcon(icon, color, size);
        //}
        #endregion

        #region Private
        private static readonly Lazy<PrivateFontCollection> Fonts = new(InitializeFonts);
        private static readonly Lazy<System.Drawing.FontFamily> FallbackFont = new(() => Fonts.Value.Families[0]);
        internal const int DefaultSize = IconHelper.DefaultSize;
        private static readonly System.Drawing.Color DefaultColor = System.Drawing.SystemColors.WindowText;
        private static readonly System.Drawing.Brush DefaultBrush = new SolidBrush(DefaultColor);

        private static PointF GetTopLeft(this Graphics graphics, string text, System.Drawing.Font font, SizeF size)
        {
            // cf.: https://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations
            var iconSize = graphics.GetIconSize(text, font, size);
            // center icon
            var left = Math.Max(0f, (size.Width - iconSize.Width) / 2);
            var top = Math.Max(0f, (size.Height - iconSize.Height) / 2);
            return new PointF(left, top);
        }

        private static SizeF GetIconSize(this Graphics graphics, string text, System.Drawing.Font font, SizeF size)
        {
            var format = new StringFormat();
            var ranges = new[] { new CharacterRange(0, text.Length) };
            format.SetMeasurableCharacterRanges(ranges);
            format.Alignment = StringAlignment.Center;
            var iconSize = graphics.MeasureString(text, font, size, format);
            return iconSize;
        }

        private static System.Drawing.Font GetAdjustedIconFont(this Graphics g, System.Drawing.FontFamily fontFamily, string text,
            SizeF size, int maxFontSize = 0, int minFontSize = 4, bool smallestOnFail = true)
        {
            var safeMaxFontSize = maxFontSize > 0 ? maxFontSize : size.Height;
            for (double adjustedSize = safeMaxFontSize; adjustedSize >= minFontSize; adjustedSize -= 0.5)
            {
                var font = GetIconFont(fontFamily, (float)adjustedSize);
                // Test the string with the new size
                var iconSize = g.GetIconSize(text, font, size);
                if (iconSize.Width < size.Width && iconSize.Height < size.Height)
                    return font;
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(fontFamily, smallestOnFail ? minFontSize : maxFontSize);
        }

        internal static System.Drawing.FontFamily FontFamilyFor(this IconChar iconChar)
        {
            if (Fonts.Value == null) return Throw("FontAwesome source font files not found!");
            var name = IconHelper.FontFor(iconChar)?.Source;
            if (name == null) return FallbackFont.Value;
            return Fonts.Value.Families.FirstOrDefault(f => name.EndsWith(f.Name, StringComparison.InvariantCultureIgnoreCase)) ?? FallbackFont.Value;
        }

        internal static System.Drawing.FontFamily FontFamilyFor(this IconChar iconChar, IconFont iconFont)
        {
            if (iconFont == IconFont.Auto) return FontFamilyFor(iconChar);
            var key = (int)iconFont;
            if (FontForStyle.TryGetValue(key, out var fontFamily)) return fontFamily;
            if (!IconHelper.FontTitles.TryGetValue((int)iconFont, out var name))
                return Throw($"No font loaded for style: {iconFont}");

            fontFamily = Fonts.Value.Families.FirstOrDefault(f => f.Name.Equals(name));
            if (fontFamily == null)
                return Throw($"No font loaded for '{name}'");

            FontForStyle.Add(key, fontFamily);
            return fontFamily;
        }

        internal static System.Drawing.FontFamily Throw(string message)
        {
            if (ThrowOnNullFonts) throw new InvalidOperationException(message);
            return default;
        }

        private static readonly Dictionary<int, System.Drawing.FontFamily> FontForStyle = new();

        private static System.Drawing.Font GetIconFont(System.Drawing.FontFamily fontFamily, float size)
        {
            return new System.Drawing.Font(fontFamily, size, GraphicsUnit.Point);
        }

        public static System.Drawing.FontFamily LoadResourceFont(this Assembly assembly, string path, string fontFile)
        {
            var fonts = new PrivateFontCollection();
            AddFont(fonts, fontFile, assembly, path);
            return fonts.Families[0];
        }

        public static unsafe void AddFont(this PrivateFontCollection fonts, string fontFile,
            Assembly assembly = null, string path = "Fonts")
        {
            var fontBytes = GetFontBytes(fontFile, assembly, path);
            fixed (byte* pFontData = fontBytes)
            {
                fonts.AddMemoryFont((IntPtr)pFontData, fontBytes.Length);
                uint dummy = 0;
                NativeMethods.AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBytes.Length, IntPtr.Zero,
                    ref dummy);
            }
        }

        private static PrivateFontCollection InitializeFonts()
        {
            var fontFiles = new[] { "fa-solid-900.ttf", "fa-regular-400.ttf", "fa-brands-400.ttf" };
            var fonts = new PrivateFontCollection();
            foreach (var fontFile in fontFiles.Reverse())
            {
                try
                {
                    AddFont(fonts, fontFile);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Could not load FontAwesome: {ex}");
                    throw;
                }
            }
            return fonts;
        }

        private static byte[] GetFontBytes(string fontFile,
            Assembly assembly = null, string path = "fonts")
        {
            var safeAssembly = assembly ?? typeof(FormsIconHelper).Assembly;
            var relativeUri = new Uri($"./{safeAssembly.GetName().Name};component/{path}/{fontFile}", UriKind.Relative);
            var uri = new Uri(IconHelper.BaseUri, relativeUri);
            var streamInfo = Application.GetResourceStream(uri);
            // ReSharper disable once PossibleNullReferenceException
            using (streamInfo.Stream)
            {
                return ReadAllBytes(streamInfo.Stream);
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using (memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static IntPtr CreateMemoryHdc(IntPtr hdc, int width, int height, out IntPtr dib)
        {
            // Create a memory DC so we can work off-screen
            var memoryHdc = NativeMethods.CreateCompatibleDC(hdc);
            NativeMethods.SetBkMode(memoryHdc, 1);

            // Create a device-independent bitmap and select it into our DC
            var info = new NativeMethods.BitMapInfo();
            info.biSize = Marshal.SizeOf(info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB
            dib = NativeMethods.CreateDIBSection(hdc, ref info, 0, out _, IntPtr.Zero, 0);
            NativeMethods.SelectObject(memoryHdc, dib);

            return memoryHdc;
        }
        #endregion
    }
}
