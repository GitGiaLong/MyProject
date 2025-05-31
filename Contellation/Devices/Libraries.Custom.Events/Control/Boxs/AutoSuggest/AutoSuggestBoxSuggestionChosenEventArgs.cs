using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Control.Boxs.AutoSuggest
{

    /// <summary>
    /// Provides data for the <see cref="AutoSuggestBox.SuggestionChosen"/> event.
    /// </summary>
    public sealed class AutoSuggestBoxSuggestionChosenEventArgs : RoutedEventArgs
    {
        public AutoSuggestBoxSuggestionChosenEventArgs(RoutedEvent eventArgs, object sender) : base(eventArgs, sender) { }

        [Required]
        public object SelectedItem { get; init; }
    }
}
