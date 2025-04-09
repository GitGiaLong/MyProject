using Libraries.Custom.Enums.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Libraries.Custom.Events.Dialog
{

    public class ContentDialogButtonClickEventArgs : RoutedEventArgs
    {
        public ContentDialogButtonClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        [Required]
        public ContentDialogButton Button { get; init; }
    }
}
