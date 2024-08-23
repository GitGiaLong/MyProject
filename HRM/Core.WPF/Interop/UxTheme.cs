using Core.WPF.Properties;

namespace Core.WPF.Interop
{
    /// ReSharper disable IdentifierTypo
    /// ReSharper disable InconsistentNaming
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

    internal static class UxTheme
    {
        /// <summary> Returned by the GetThemeMargins function to define the margins of windows that have visual styles applied. </summary>
        public struct MARGINS
        {
            /// <summary> Width of left border that retains its size. </summary>
            public int cxLeftWidth;

            /// <summary> Width of right border that retains its size. </summary>
            public int cxRightWidth;

            /// <summary> Height of top border that retains its size. </summary>
            public int cyTopHeight;

            /// <summary> Height of bottom border that retains its size. </summary>
            public int cyBottomHeight;
        }

        /// <summary> Specifies the Type of visual style attribute to set on a window. </summary>
        public enum WINDOWTHEMEATTRIBUTETYPE : uint
        {
            /// <summary> Non-client area window attributes will be set. </summary>
            WTA_NONCLIENT = 1,
        }

        /// <summary> WindowThemeNonClientAttributes </summary>
        [Flags]
        public enum WTNCA : uint
        {
            /// <summary> Prevents the window caption from being drawn. </summary>
            NODRAWCAPTION = 0x00000001,

            /// <summary> Prevents the system icon from being drawn. </summary>
            NODRAWICON = 0x00000002,

            /// <summary> Prevents the system icon menu from appearing. </summary>
            NOSYSMENU = 0x00000004,

            /// <summary> Prevents mirroring of the question mark, even in right-to-left (RTL) layout. </summary>
            NOMIRRORHELP = 0x00000008,

            /// <summary> A mask that contains all the valid bits. </summary>
            VALIDBITS = NODRAWCAPTION | NODRAWICON | NOMIRRORHELP | NOSYSMENU,
        }

        /// <summary> Defines options that are used to set window visual style attributes. </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct WTA_OPTIONS
        {
            // public static readonly uint Size = (uint)Marshal.SizeOf(typeof(WTA_OPTIONS));
            public const uint Size = 8;

            /// <summary>
            /// A combination of flags that modify window visual style attributes.
            /// Can be a combination of the WTNCA constants.
            /// </summary>
            [FieldOffset(0)]
            public WTNCA dwFlags;

            /// <summary>
            /// A bitmask that describes how the values specified in dwFlags should be applied.
            /// If the bit corresponding to a value in dwFlags is 0, that flag will be removed.
            /// If the bit is 1, the flag will be added.
            /// </summary>
            [FieldOffset(4)]
            public WTNCA dwMask;
        }

        /// <summary>
        /// Sets attributes to control how visual styles are applied to a specified window.
        /// </summary>
        /// <param Name="hWnd">
        /// Handle to a window to apply changes to.
        /// </param>
        /// <param Name="eAttribute">
        /// Value of Type WINDOWTHEMEATTRIBUTETYPE that specifies the Type of attribute to set.
        /// The value of this parameter determines the Type of data that should be passed in the pvAttribute parameter.
        /// Can be the following value:
        /// <list>WTA_NONCLIENT (Specifies non-client related attributes).</list>
        /// pvAttribute must be a pointer of Type WTA_OPTIONS.
        /// </param>
        /// <param Name="pvAttribute">
        /// A pointer that specifies attributes to set. Type is determined by the value of the eAttribute value.
        /// </param>
        /// <param Name="cbAttribute">
        /// Specifies the size, in bytes, of the data pointed to by pvAttribute.
        /// </param>
        [DllImport(Libraries.UxTheme, PreserveSig = false)]
        public static extern void SetWindowThemeAttribute([In] IntPtr hWnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

        /// <summary>
        /// Tests if a visual style for the current application is active.
        /// </summary>
        /// <returns><see langword="true"/> if a visual style is enabled, and windows with visual styles applied should call OpenThemeData to start using theme drawing services.</returns>
        [DllImport(Libraries.UxTheme)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsThemeActive();

        /// <summary>
        /// Retrieves the Name of the current visual style, and optionally retrieves the color scheme Name and size Name.
        /// </summary>
        /// <param Name="pszThemeFileName">Pointer to a string that receives the theme path and file Name.</param>
        /// <param Name="dwMaxNameChars">Value of Type int that contains the maximum number of characters allowed in the theme file Name.</param>
        /// <param Name="pszColorBuff">Pointer to a string that receives the color scheme Name. This parameter may be set to NULL.</param>
        /// <param Name="cchMaxColorChars">Value of Type int that contains the maximum number of characters allowed in the color scheme Name.</param>
        /// <param Name="pszSizeBuff">Pointer to a string that receives the size Name. This parameter may be set to NULL.</param>
        /// <param Name="cchMaxSizeChars">Value of Type int that contains the maximum number of characters allowed in the size Name.</param>
        /// <returns>HRESULT</returns>
        [DllImport(Libraries.UxTheme, CharSet = CharSet.Unicode)]
        public static extern int GetCurrentThemeName([Out] StringBuilder pszThemeFileName, [In] int dwMaxNameChars, [Out] StringBuilder pszColorBuff, [In] int cchMaxColorChars,
            [Out] StringBuilder pszSizeBuff, [In] int cchMaxSizeChars);
    }

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
}
