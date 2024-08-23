using Core.WPF.Appearance;
using Core.WPF.Appearance.Enums;

namespace Core.WPF.Markup
{
    /// <summary>
    /// Provides a dictionary implementation that contains <c>Core.WPF</c> 
    /// theme resources used by components and other elements of a WPF application.
    /// </summary>
    [Localizability(LocalizationCategory.Ignore)]
    [Ambient]
    [UsableDuringInitialization(true)]
    public class ThemesDictionary : ResourceDictionary
    {
        /// <summary>
        /// Sets the default application theme.
        /// </summary>
        public ApplicationTheme Theme
        {
            set => SetSourceBasedOnSelectedTheme(value);
        }

        public ThemesDictionary()
        {
            SetSourceBasedOnSelectedTheme(ApplicationTheme.Light);
        }

        private void SetSourceBasedOnSelectedTheme(ApplicationTheme? selectedApplicationTheme)
        {
            var themeName = selectedApplicationTheme switch
            {
                ApplicationTheme.Dark => "Dark",
                ApplicationTheme.HighContrast => "HighContrast",
                _ => "Light"
            };

            Source = new Uri($"{ApplicationThemeManager.ThemesDictionaryPath}{themeName}.xaml", UriKind.Absolute);
        }
    }
}
