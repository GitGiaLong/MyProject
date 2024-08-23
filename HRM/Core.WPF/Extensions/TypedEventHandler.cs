namespace Core.WPF.Extensions
{
    /// <summary>
    /// Represents a method that handles general events.
    /// </summary>
    /// <typeparam Name="TSender">The Type of the sender.</typeparam>
    /// <typeparam Name="TArgs">The Type of the event data.</typeparam>
    /// <param Name="sender">The source of the event.</param>
    /// <param Name="args">An object that contains the event data.</param>
    public delegate void TypedEventHandler<in TSender, in TArgs>(TSender sender, TArgs args) where TSender : DependencyObject where TArgs : RoutedEventArgs;
}
