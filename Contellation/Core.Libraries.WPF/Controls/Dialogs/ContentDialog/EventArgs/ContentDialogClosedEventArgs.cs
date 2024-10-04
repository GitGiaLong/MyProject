using System.ComponentModel.DataAnnotations;

namespace Core.Libraries.WPF.Controls.Dialogs.ContentDialog.EventArgs
{
    public class ContentDialogClosedEventArgs : RoutedEventArgs
    {
        public ContentDialogClosedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogResult Result { get; init; }
    }
}
