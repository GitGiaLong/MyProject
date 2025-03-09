using Libraries.Custom.Properties;
using Libraries.Custom.Structs;

namespace Libraries.Custom.Icons.Methods
{
    public class NativeMethod
    {
        //[DllImport("gdi32.dll")]
        [DllImport(LibrariesDLL.Gdi32)]
        public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        [DllImport(LibrariesDLL.Gdi32)]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport(LibrariesDLL.Gdi32, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport(LibrariesDLL.Gdi32)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage,
            out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport(LibrariesDLL.Gdi32)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

        [DllImport(LibrariesDLL.Gdi32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport(LibrariesDLL.Gdi32)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(LibrariesDLL.Gdi32, ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);
    }
}
