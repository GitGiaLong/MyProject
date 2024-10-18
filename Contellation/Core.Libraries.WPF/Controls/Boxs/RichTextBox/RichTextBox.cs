namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Extends the <see cref="System.Windows.Controls.RichTextBox"/> control with additional properties.
    /// </summary>
    public class RichTextBox : System.Windows.Controls.RichTextBox
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user can select text in the control.
        /// </summary>
        public bool IsTextSelectionEnabled
        {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(nameof(IsTextSelectionEnabled), typeof(bool),
            typeof(RichTextBox), new PropertyMetadata(false));
    }
}
