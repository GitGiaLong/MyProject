using System.ComponentModel.DataAnnotations;

namespace Core.Libraries.WPF.Controls.Dialogs.ContentDialog.EventArgs
{
    public class ContentDialogButtonClickEventArgs : RoutedEventArgs
    {
        public ContentDialogButtonClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogButton Button { get; init; }
    }
}
