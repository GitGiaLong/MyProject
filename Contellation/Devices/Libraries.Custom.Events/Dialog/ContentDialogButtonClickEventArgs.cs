using Libraries.Custom.Enums.Dialog;
using System.ComponentModel.DataAnnotations;

namespace Libraries.Custom.Events.Dialog
{

    public class ContentDialogButtonClickEventArgs : RoutedEventArgs
    {
        public ContentDialogButtonClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogButton Button { get; init; }
    }
}
