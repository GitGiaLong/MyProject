using Libraries.Custom.Enums.Dialog;
using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Dialog
{
    public class ContentDialogClosingEventArgs : RoutedEventArgs
    {
        public ContentDialogClosingEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogResult Result { get; init; }

        public bool Cancel { get; set; }
    }
}
