﻿using System.Windows.Controls;
using System.Windows.Input;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>A custom ScrollViewer that allows certain mouse events to bubble through when it's inactive.</summary>
    public class PassiveScrollViewer : ScrollViewer
    {
        /// <summary>Gets or sets a value indicating whether blocked inner scrolling should be propagated forward.</summary>
        public bool IsScrollSpillEnabled
        {
            get { return (bool)GetValue(IsScrollSpillEnabledProperty); }
            set { SetValue(IsScrollSpillEnabledProperty, value); }
        }
        /// <summary>Identifies the <see cref="IsScrollSpillEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsScrollSpillEnabledProperty = DependencyProperty.Register(nameof(IsScrollSpillEnabled), typeof(bool),
            typeof(PassiveScrollViewer), new PropertyMetadata(true));

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (IsVerticalScrollingDisabled || IsContentSmallerThanViewport || (IsScrollSpillEnabled && HasReachedEndOfScrolling(e))) { return; }

            base.OnMouseWheel(e);
        }

        private bool IsVerticalScrollingDisabled => VerticalScrollBarVisibility == ScrollBarVisibility.Disabled;

        private bool IsContentSmallerThanViewport => ScrollableHeight <= 0;

        private bool HasReachedEndOfScrolling(MouseWheelEventArgs e)
        {
            var isScrollingUp = e.Delta > 0;
            var isScrollingDown = e.Delta < 0;
            var isTopOfViewport = VerticalOffset == 0;
            var isBottomOfViewport = VerticalOffset >= ScrollableHeight;

            return (isScrollingUp && isTopOfViewport) || (isScrollingDown && isBottomOfViewport);
        }
    }
}
