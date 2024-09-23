namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Use <see cref="ToggleSwitch"/> to present users with two mutally exclusive options (like on/off).
    /// </summary>
    public class ToggleSwitch : System.Windows.Controls.Primitives.ToggleButton
    {
        /// <summary>
        /// Gets or sets the content that should be displayed when the <see cref="ToggleSwitch"/> is in the "Off" state.
        /// </summary>
        public object? OffContent
        {
            get { return GetValue(OffContentProperty); }
            set { SetValue(OffContentProperty, value); }
        }
        /// <summary>Identifies the <see cref="OffContent"/> dependency property.</summary>
        public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(nameof(OffContent), typeof(object),
            typeof(ToggleSwitch), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content that should be displayed when the <see cref="ToggleSwitch"/> is in the "On" state.
        /// </summary>
        public object? OnContent
        {
            get { return GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }
        /// <summary>Identifies the <see cref="OnContent"/> dependency property.</summary>
        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(nameof(OnContent), typeof(object),
            typeof(ToggleSwitch), new PropertyMetadata(null));
    }
}
