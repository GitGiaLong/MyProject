using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Control.Navigation.NavigationView
{
    public class NavigatedEventArgs : RoutedEventArgs
    {
        public NavigatedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public object Page { get; init; }

    }
}
