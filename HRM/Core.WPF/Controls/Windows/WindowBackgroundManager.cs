using Core.WPF.Appearance.Enums;
using Core.WPF.Controls.Enums.Window;
using Core.WPF.Interop;

namespace Core.WPF.Controls.Windows
{
    /// <summary> Facilitates the management of the window background. </summary>
    public static class WindowBackgroundManager
    {
        /// <summary>Tries to apply dark theme to <see cref="Window"/>.</summary>
        public static void ApplyDarkThemeToWindow(Window? window)
        {
            if (window is null) { return; }

            if (window.IsLoaded) { _ = UnsafeNativeMethods.ApplyWindowDarkMode(window); }

            window.Loaded += (sender, _) => UnsafeNativeMethods.ApplyWindowDarkMode(sender as Window);
        }

        /// <summary> Tries to remove dark theme from <see cref="Window"/>. </summary>
        public static void RemoveDarkThemeFromWindow(Window? window)
        {
            if (window is null) { return; }

            if (window.IsLoaded) { _ = UnsafeNativeMethods.RemoveWindowDarkMode(window); }

            window.Loaded += (sender, _) => UnsafeNativeMethods.RemoveWindowDarkMode(sender as Window);
        }

        [Obsolete("Use UpdateBackground(Window, ApplicationTheme, WindowBackdropType) instead.")]
        public static void UpdateBackground(Window? window, ApplicationTheme applicationTheme, WindowBackdropType backdrop, bool forceBackground)
        {
            UpdateBackground(window, applicationTheme, backdrop);
        }

        /// <summary> Forces change to application background. Required if custom background effect was previously applied. </summary>
        public static void UpdateBackground(Window? window, ApplicationTheme applicationTheme, WindowBackdropType backdrop)
        {
            if (window is null) { return; }

            _ = WindowBackdrop.RemoveBackdrop(window);

            if (applicationTheme == ApplicationTheme.HighContrast) { backdrop = WindowBackdropType.None; }

            _ = WindowBackdrop.ApplyBackdrop(window, backdrop);
            if (applicationTheme is ApplicationTheme.Dark) { ApplyDarkThemeToWindow(window); }
            else { RemoveDarkThemeFromWindow(window); }

            foreach (object? subWindow in window.OwnedWindows)
            {
                if (subWindow is Window windowSubWindow)
                {
                    _ = WindowBackdrop.ApplyBackdrop(windowSubWindow, backdrop);

                    if (applicationTheme is ApplicationTheme.Dark) { ApplyDarkThemeToWindow(windowSubWindow); }
                    else { RemoveDarkThemeFromWindow(windowSubWindow); }
                }
            }
        }
    }
}
