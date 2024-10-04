using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Libraries.WPF.Controls.Bars.Menu
{

    /// <summary>
    /// Extended <see cref="System.Windows.Controls.MenuItem"/> with <see cref="SymbolRegular"/> properties.
    /// </summary>
    public class MenuItem : System.Windows.Controls.MenuItem
    {
        static MenuItem()
        {
            IconProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null));
        }

        /// <summary>
        /// Gets or sets displayed <see cref="Icon"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "WpfAnalyzers.DependencyProperty",
            "WPF0012:CLR property type should match registered type",
            Justification = "seems harmless"
        )]
        public new string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
