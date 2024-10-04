using System.ComponentModel.DataAnnotations;

namespace Core.Libraries.WPF.Controls.Dialogs.ContentDialog.EventArgs
{
    public class ContentDialogClosingEventArgs : RoutedEventArgs
    {
        public ContentDialogClosingEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source) { }

        [Required]
        public  ContentDialogResult Result { get; init; }

        public bool Cancel { get; set; }
    }
}
