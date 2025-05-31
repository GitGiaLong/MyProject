using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Control.Boxs.AutoSuggest
{
    /// <summary>
    /// Provides event data for the <see cref="AutoSuggestBox.QuerySubmitted"/> event.
    /// </summary>
    public sealed class AutoSuggestBoxQuerySubmittedEventArgs : RoutedEventArgs
    {
        public AutoSuggestBoxQuerySubmittedEventArgs(RoutedEvent eventArgs, object sender) : base(eventArgs, sender) { }

        [Required]
        public string QueryText { get; init; }
    }
}
