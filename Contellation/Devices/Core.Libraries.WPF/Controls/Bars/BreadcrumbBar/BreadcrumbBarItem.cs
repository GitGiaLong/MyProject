namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="BreadcrumbBar"/> control.
    /// </summary>
    public class BreadcrumbBarItem : System.Windows.Controls.ContentControl
    {
        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(BreadcrumbBarItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets get or sets margin for the <see cref="Icon"/>
        /// </summary>
        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }
        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(nameof(IconMargin), typeof(Thickness),
            typeof(BreadcrumbBarItem), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Gets or sets a value indicating whether the current item is the last one.
        /// </summary>
        public bool IsLast
        {
            get { return (bool)GetValue(IsLastProperty); }
            set { SetValue(IsLastProperty, value); }
        }
        public static readonly DependencyProperty IsLastProperty = DependencyProperty.Register(nameof(IsLast), typeof(bool),
            typeof(BreadcrumbBarItem), new PropertyMetadata(false));

    }
}
