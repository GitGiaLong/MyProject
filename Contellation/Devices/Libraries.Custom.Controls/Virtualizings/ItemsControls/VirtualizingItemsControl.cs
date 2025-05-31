using System.Windows.Controls;

namespace Libraries.Custom.Controls
{
    /// <summary>
    /// Virtualized <see cref="ItemsControl"/>.
    /// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
    /// </summary>
    public class VirtualizingItemsControl : System.Windows.Controls.ItemsControl
    {
        /// <summary>
        /// Gets or sets the cache length unit.
        /// </summary>
        public VirtualizationCacheLengthUnit CacheLengthUnit
        {
            get => VirtualizingPanel.GetCacheLengthUnit(this);
            set
            {
                SetValue(CacheLengthUnitProperty, value);
                VirtualizingPanel.SetCacheLengthUnit(this, value);
            }
        }
        /// <summary>Identifies the <see cref="CacheLengthUnit"/> dependency property.</summary>
        public static readonly DependencyProperty CacheLengthUnitProperty = DependencyProperty.Register(nameof(CacheLengthUnit),
            typeof(VirtualizationCacheLengthUnit), typeof(VirtualizingItemsControl), new FrameworkPropertyMetadata(VirtualizationCacheLengthUnit.Page));

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingItemsControl"/> class.
        /// </summary>
        public VirtualizingItemsControl()
        {
            VirtualizingPanel.SetCacheLengthUnit(this, CacheLengthUnit);
            VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));
            VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
        }
    }
}
