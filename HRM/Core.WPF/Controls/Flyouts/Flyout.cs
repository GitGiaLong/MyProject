using Core.WPF.Extensions;
using System.Windows.Controls.Primitives;

namespace Core.WPF.Controls
{
    /// <summary>
    /// Represents a control that creates a pop-up window that displays information for an element in the interface.
    /// </summary>
    [TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
    public class Flyout : System.Windows.Controls.ContentControl
    {
        private const string ElementPopup = "PART_Popup";

        private System.Windows.Controls.Primitives.Popup? _popup = default;

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="Flyout" /> is visible.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary>Identifies the <see cref="IsOpen"/> dependency property.</summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool),
            typeof(Flyout), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the orientation of the <see cref="Flyout" /> control when the control opens,
        /// and specifies the behavior of the <see cref="T:System.Windows.Controls.Primitives.Popup" />
        /// control when it overlaps screen boundaries.
        /// </summary>
        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        /// <summary>Identifies the <see cref="Placement"/> dependency property.</summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(nameof(Placement), typeof(PlacementMode),
            typeof(Flyout), new PropertyMetadata(PlacementMode.Top));

        /// <summary>
        /// Event triggered when <see cref="Flyout" /> is opened.
        /// </summary>
        public event TypedEventHandler<Flyout, RoutedEventArgs> Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }
        /// <summary>Identifies the <see cref="Opened"/> routed event.</summary>
        public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<Flyout, RoutedEventArgs>), typeof(Flyout));

        /// <summary>
        /// Event triggered when <see cref="Flyout" /> is opened.
        /// </summary>
        public event TypedEventHandler<Flyout, RoutedEventArgs> Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
        /// <summary>Identifies the <see cref="Closed"/> routed event.</summary>
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<Flyout, RoutedEventArgs>), typeof(Flyout));

        /// <summary>
        /// Invoked whenever application code or an internal process,
        /// such as a rebuilding layout pass, calls the ApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = GetTemplateChild(ElementPopup) as System.Windows.Controls.Primitives.Popup;

            if (_popup is null) { return; }

            _popup.Opened -= OnPopupOpened;
            _popup.Opened += OnPopupOpened;

            _popup.Closed -= OnPopupClosed;
            _popup.Closed += OnPopupClosed;
        }

        public void Show() { if (!IsOpen) { SetCurrentValue(IsOpenProperty, true); } }

        public void Hide() { if (IsOpen) { SetCurrentValue(IsOpenProperty, false); } }

        protected virtual void OnPopupOpened(object? sender, EventArgs e) { RaiseEvent(new RoutedEventArgs(OpenedEvent, this)); }

        protected virtual void OnPopupClosed(object? sender, EventArgs e)
        {
            Hide();
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }
    }
}
