using System.Windows;

namespace GSMFWPF.FontsIcon
{
#if !WINDOWS_UWP && !WINUI
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
#endif
    public enum EFlipOrientation
    {
        /// <summary>
        /// Default
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Flip horizontally (on x-achsis)
        /// </summary>
        Horizontal,
        /// <summary>
        /// Flip vertically (on y-achsis)
        /// </summary>
        Vertical,
    }
    /// <summary>
    /// Represents a flippable control
    /// </summary>
    public interface IFlippable
    {
        /// <summary>
        /// Gets or sets the current orientation (horizontal, vertical).
        /// </summary>
        EFlipOrientation FlipOrientation { get; set; }
    }
}
