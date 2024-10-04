using System.ComponentModel;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TreeViewItem"/> with <see cref="SymbolRegular"/> properties.
    /// </summary>
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        /// <summary>
        /// Gets or sets displayed <see cref="Icon"/>.
        /// </summary>
        public string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(TreeViewItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets displayed <see cref="FontFamily"/>.
        /// </summary>
        public FontFamily? FontFamilyIcon
        {
            get => (FontFamily?)GetValue(FontFamilyIconProperty);
            set => SetValue(FontFamilyIconProperty, value);
        }
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty FontFamilyIconProperty = DependencyProperty.Register(nameof(FontFamilyIcon), typeof(FontFamily),
            typeof(TreeViewItem), new PropertyMetadata(null));
    }
}
