﻿namespace Core.Libraries.WPF.Controls
{

    /// <summary>
    /// Simple Card with content and <see cref="Footer"/>.
    /// </summary>
    public class Card : System.Windows.Controls.ContentControl
    {
        /// <summary>Identifies the <see cref="Footer"/> dependency property.</summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            nameof(Footer),
            typeof(object),
            typeof(Card),
            new PropertyMetadata(null, OnFooterChanged)
        );

        /// <summary>Identifies the <see cref="HasFooter"/> dependency property.</summary>
        public static readonly DependencyProperty HasFooterProperty = DependencyProperty.Register(
            nameof(HasFooter),
            typeof(bool),
            typeof(Card),
            new PropertyMetadata(false)
        );

        /// <summary>
        /// Gets or sets additional content displayed at the bottom.
        /// </summary>
        public object? Footer
        {
            get => GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Card"/> has a <see cref="Footer"/>.
        /// </summary>
        public bool HasFooter
        {
            get => (bool)GetValue(HasFooterProperty);
            internal set => SetValue(HasFooterProperty, value);
        }

        private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Card control)
            {
                return;
            }

            control.SetValue(HasFooterProperty, control.Footer != null);
        }
    }
}
