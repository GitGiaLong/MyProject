using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Control.Navigation.NavigationView
{
    public class NavigatingCancelEventArgs : RoutedEventArgs
    {
        public NavigatingCancelEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source) { }

        [Required]
        public object Page { get; init; }

        public bool Cancel { get; set; }
    }
}
