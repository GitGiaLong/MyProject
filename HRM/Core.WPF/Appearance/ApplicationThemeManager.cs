﻿using Core.WPF.Appearance.Enums;
using Core.WPF.Controls.Enums.Window;
using Core.WPF.Controls.Windows;
using Core.WPF.Properties;

namespace Core.WPF.Appearance
{
    /// <summary>
    /// Allows to manage the application theme by swapping resource dictionaries containing dynamic resources with color information.
    /// </summary>
    public static class ApplicationThemeManager
    {
        private static ApplicationTheme _cachedApplicationTheme = ApplicationTheme.Unknown;

        internal const string LibraryNamespace = "Generic;";

        internal const string ThemesDictionaryPath = "pack://application:,,,/Core.WPF;component/Themes/";

        /// <summary> Event triggered when the application's theme is changed. </summary>
        public static event ThemeChangedEvent? Changed;

        /// <summary>
        /// Gets a value that indicates whether the application is currently using the high contrast theme.
        /// </summary>
        /// <returns><see langword="true"/> if application uses high contrast theme.</returns>
        public static bool IsHighContrast() => _cachedApplicationTheme == ApplicationTheme.HighContrast;

        /// <summary>
        /// Gets a value that indicates whether the Windows is currently using the high contrast theme.
        /// </summary>
        /// <returns><see langword="true"/> if system uses high contrast theme.</returns>
        public static bool IsSystemHighContrast() => SystemThemeManager.HighContrast;

        /// <summary>
        /// Changes the current application theme.
        /// </summary>
        /// <param Name="applicationTheme">Theme to set.</param>
        /// <param Name="backgroundEffect">Whether the custom background effect should be applied.</param>
        /// <param Name="updateAccent">Whether the color accents should be changed.</param>
        public static void Apply(ApplicationTheme applicationTheme, WindowBackdropType backgroundEffect = WindowBackdropType.Mica, bool updateAccent = true)
        {
            if (updateAccent) { ApplicationAccentColorManager.Apply(ApplicationAccentColorManager.GetColorizationColor(), applicationTheme, false); }

            if (applicationTheme == ApplicationTheme.Unknown) { return; }

            var appDictionaries = new ResourceDictionaryManager(LibraryNamespace);

            var themeDictionaryName = "Light";

            switch (applicationTheme)
            {
                case ApplicationTheme.Dark:
                    themeDictionaryName = "Dark";
                    break;
                case ApplicationTheme.HighContrast:
                    themeDictionaryName = ApplicationThemeManager.GetSystemTheme() switch
                    {
                        SystemTheme.HC1 => "HC1",
                        SystemTheme.HC2 => "HC2",
                        SystemTheme.HCBlack => "HCBlack",
                        SystemTheme.HCWhite => "HCBlack",
                        _ => "HCWhite",
                    };
                    break;
            }

            bool isUpdated = appDictionaries.UpdateDictionary("Themes", new Uri(ThemesDictionaryPath + themeDictionaryName + ".xaml", UriKind.Absolute));

            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ApplicationThemeManager)} tries to update theme to {themeDictionaryName} ({applicationTheme}): {isUpdated}",
                nameof(ApplicationThemeManager));

            if (!isUpdated) { return; }

            SystemThemeManager.UpdateSystemThemeCache();

            _cachedApplicationTheme = applicationTheme;

            Changed?.Invoke(applicationTheme, ApplicationAccentColorManager.SystemAccent);

            if (UiApplication.Current.MainWindow is Window mainWindow) { WindowBackgroundManager.UpdateBackground(mainWindow, applicationTheme, backgroundEffect); }
        }

        /// <summary>
        /// Applies Resources in the <paramref Name="frameworkElement"/>.
        /// </summary>
        public static void Apply(FrameworkElement frameworkElement)
        {
            if (frameworkElement is null) { return; }

            ResourceDictionary[] resourcesRemove = frameworkElement.Resources.MergedDictionaries.Where(e => e.Source is not null)
                .Where(e => e.Source.ToString().ToLower().Contains(LibraryNamespace))
                .ToArray();

            foreach (ResourceDictionary? resource in UiApplication.Current.Resources.MergedDictionaries)
            {
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ApplicationThemeManager)} Add {resource.Source}", "CoreLibUI.Appearance");
                frameworkElement.Resources.MergedDictionaries.Add(resource);
            }

            foreach (ResourceDictionary resource in resourcesRemove)
            {
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ApplicationThemeManager)} Remove {resource.Source}", "CoreLibUI.Appearance");

                _ = frameworkElement.Resources.MergedDictionaries.Remove(resource);
            }

            foreach (System.Collections.DictionaryEntry resource in UiApplication.Current.Resources)
            {
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ApplicationThemeManager)} Copy Resource {resource.Key} - {resource.Value}", "CoreLibUI.Appearance");
                frameworkElement.Resources[resource.Key] = resource.Value;
            }
        }

        public static void ApplySystemTheme() { ApplySystemTheme(true); }

        public static void ApplySystemTheme(bool updateAccent)
        {
            SystemThemeManager.UpdateSystemThemeCache();

            SystemTheme systemTheme = GetSystemTheme();

            ApplicationTheme themeToSet = ApplicationTheme.Light;

            if (systemTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow) { themeToSet = ApplicationTheme.Dark; }
            else if (systemTheme is SystemTheme.HC1 or SystemTheme.HC2 or SystemTheme.HCBlack or SystemTheme.HCWhite) { themeToSet = ApplicationTheme.HighContrast; }

            Apply(themeToSet, updateAccent: updateAccent);
        }

        /// <summary>
        /// Gets currently set application theme.
        /// </summary>
        /// <returns><see cref="ApplicationTheme.Unknown"/> if something goes wrong.</returns>
        public static ApplicationTheme GetAppTheme()
        {
            if (_cachedApplicationTheme == ApplicationTheme.Unknown) { FetchApplicationTheme(); }

            return _cachedApplicationTheme;
        }

        /// <summary>
        /// Gets currently set system theme.
        /// </summary>
        /// <returns><see cref="SystemTheme.Unknown"/> if something goes wrong.</returns>
        public static SystemTheme GetSystemTheme() { return SystemThemeManager.GetCachedSystemTheme(); }

        /// <summary>
        /// Gets a value that indicates whether the application is matching the system theme.
        /// </summary>
        /// <returns><see langword="true"/> if the application has the same theme as the system.</returns>
        public static bool IsAppMatchesSystem()
        {
            ApplicationTheme appApplicationTheme = GetAppTheme();
            SystemTheme sysTheme = GetSystemTheme();

            return appApplicationTheme switch
            {
                ApplicationTheme.Dark => sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow,
                ApplicationTheme.Light => sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise,
                _ => appApplicationTheme == ApplicationTheme.HighContrast && SystemThemeManager.HighContrast
            };
        }

        /// <summary> Checks if the application and the operating system are currently working in a dark theme. </summary>
        public static bool IsMatchedDark()
        {
            ApplicationTheme appApplicationTheme = GetAppTheme();
            SystemTheme sysTheme = GetSystemTheme();

            if (appApplicationTheme != ApplicationTheme.Dark) { return false; }

            return sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow;
        }

        /// <summary> Checks if the application and the operating system are currently working in a light theme. </summary>
        public static bool IsMatchedLight()
        {
            ApplicationTheme appApplicationTheme = GetAppTheme();
            SystemTheme sysTheme = GetSystemTheme();

            if (appApplicationTheme != ApplicationTheme.Light) { return false; }

            return sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise;
        }

        /// <summary>Tries to guess the currently set application theme.</summary>
        private static void FetchApplicationTheme()
        {
            ResourceDictionaryManager appDictionaries = new(LibraryNamespace);
            ResourceDictionary? themeDictionary = appDictionaries.GetDictionary("theme");

            if (themeDictionary == null) { return; }

            string themeUri = themeDictionary.Source.ToString().Trim().ToLower();

            if (themeUri.Contains("light")) { _cachedApplicationTheme = ApplicationTheme.Light; }

            if (themeUri.Contains("dark")) { _cachedApplicationTheme = ApplicationTheme.Dark; }

            if (themeUri.Contains("highcontrast")) { _cachedApplicationTheme = ApplicationTheme.HighContrast; }
        }
    }
}
