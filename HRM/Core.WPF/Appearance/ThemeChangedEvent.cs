using Core.WPF.Appearance.Enums;

namespace Core.WPF.Appearance
{
    /// <summary>
    /// Event triggered when application theme is updated.
    /// </summary>
    /// <param Name="currentApplicationTheme">Current application <see cref="ApplicationTheme"/>.</param>
    /// <param Name="systemAccent">Current base system accent <see cref="Color"/>.</param>
    public delegate void ThemeChangedEvent(ApplicationTheme currentApplicationTheme, Color systemAccent);
}
