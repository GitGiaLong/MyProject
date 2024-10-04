using System.ComponentModel.DataAnnotations;

namespace Core.Libraries.WPF.Controls.Boxs.AutoSuggestBox
{
    /// <summary>
    /// Provides data for the <see cref="AutoSuggestBox.TextChanged"/> event.
    /// </summary>
    public sealed class AutoSuggestBoxTextChangedEventArgs : RoutedEventArgs
    {
        public AutoSuggestBoxTextChangedEventArgs(RoutedEvent eventArgs, object sender)
            : base(eventArgs, sender) { }

        [Required]
        public string Text { get; init; }

        [Required]
        public AutoSuggestionBoxTextChangeReason Reason { get; init; }
    }
}
