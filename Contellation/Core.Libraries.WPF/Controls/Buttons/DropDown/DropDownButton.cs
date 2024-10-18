﻿using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// A control that drop downs a flyout of choices from which one can be chosen.
    /// </summary>
    public class DropDownButton : Button
    {
        private ContextMenu? _contextMenu;

        /// <summary>
        /// Gets or sets the flyout associated with this button.
        /// </summary>
        public object? Flyout
        {
            get { return GetValue(FlyoutProperty); }
            set { SetValue(FlyoutProperty, value); }
        }
        public static readonly DependencyProperty FlyoutProperty = DependencyProperty.Register(nameof(Flyout), typeof(object),
            typeof(DropDownButton), new PropertyMetadata(null, OnFlyoutChanged));

        private static void OnFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropDownButton dropDownButton)
            {
                dropDownButton.OnFlyoutChanged(e.NewValue);
            }
        }
        /// <summary>
        /// This method is invoked when the <see cref="FlyoutProperty"/> changes.
        /// </summary>
        /// <param name="value">The new value of <see cref="FlyoutProperty"/>.</param>
        protected virtual void OnFlyoutChanged(object value)
        {
            if (value is ContextMenu contextMenu)
            {
                _contextMenu = contextMenu;
                _contextMenu.Opened += OnContextMenuOpened;
                _contextMenu.Closed += OnContextMenuClosed;
            }
        }
        protected virtual void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
        protected virtual void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop-down for a button is currently open.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the drop-down is open; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        /// <summary>Identifies the <see cref="IsDropDownOpen"/> dependency property.</summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool),
            typeof(DropDownButton), new PropertyMetadata(false));

        protected virtual void OnIsDropDownOpenChanged(bool currentValue) { }

        protected override void OnClick()
        {
            base.OnClick();

            if (_contextMenu is null) { return; }

            _contextMenu.SetCurrentValue(MinWidthProperty, ActualWidth);
            _contextMenu.SetCurrentValue(ContextMenu.PlacementTargetProperty, this);
            _contextMenu.SetCurrentValue(ContextMenu.PlacementProperty, System.Windows.Controls.Primitives.PlacementMode.Bottom);
            _contextMenu.SetCurrentValue(ContextMenu.IsOpenProperty, true);
        }
    }
}