using Libraries.Custom.Enums.Dialog;
using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Dialog
{
    public class ContentDialogClosedEventArgs : RoutedEventArgs
    {
        public ContentDialogClosedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogResult Result { get; init; }
    }
}
