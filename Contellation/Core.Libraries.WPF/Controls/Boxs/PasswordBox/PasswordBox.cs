﻿using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// The modified password control.
    /// </summary>
    public class PasswordBox : TextBox
    {
        private bool _lockUpdatingContents;

        /// <summary>
        /// Gets or sets currently typed text represented by asterisks.
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string),
            typeof(PasswordBox), new PropertyMetadata(string.Empty, OnPasswordChanged));

        /// <summary>
        /// Called when <see cref="Password"/> is changed.
        /// </summary>
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox control) { return; }

            control.OnPasswordChanged();
        }
        /// <summary>
        /// Is called when <see cref="Password"/> property is changing.
        /// </summary>
        protected virtual void OnPasswordChanged()
        {
            UpdateTextContents(false);
        }

        /// <summary>
        /// Gets or sets character used to mask the password.
        /// </summary>
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(nameof(PasswordChar), typeof(char),
            typeof(PasswordBox), new PropertyMetadata('●', OnPasswordCharChanged));

        /// <summary>
        /// Is called when <see cref="PasswordChar"/> property is changing.
        /// </summary>
        protected virtual void OnPasswordCharChanged()
        {
            /// If password is currently revealed,
            /// do not replace displayed text with asterisks
            if (IsPasswordRevealed) { return; }

            _lockUpdatingContents = true;

            SetCurrentValue(TextProperty, new string(PasswordChar, Password.Length));

            _lockUpdatingContents = false;
        }
        /// <summary>
        /// Called if the character is changed in the during the run.
        /// </summary>
        private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox control) { return; }

            control.OnPasswordCharChanged();
        }

        /// <summary>
        /// Gets a value indicating whether the password is revealed.
        /// </summary>
        public bool IsPasswordRevealed
        {
            get { return (bool)GetValue(IsPasswordRevealedProperty); }
            private set { SetValue(IsPasswordRevealedProperty, value); }
        }
        public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(nameof(IsPasswordRevealed), typeof(bool),
            typeof(PasswordBox), new PropertyMetadata(false, OnIsPasswordRevealedChanged));

        /// <summary>
        /// Called if the reveal mode is changed in the during the run.
        /// </summary>
        private static void OnIsPasswordRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox control) { return; }

            control.OnPasswordRevealModeChanged();
        }
        protected virtual void OnPasswordRevealModeChanged()
        {
            _lockUpdatingContents = true;

            SetCurrentValue(TextProperty, IsPasswordRevealed ? Password : new string(PasswordChar, Password.Length));

            _lockUpdatingContents = false;
        }

        public bool IsSafeEnabled
        {
            get { return (bool)GetValue(IsSafeEnabledProperty); }
            set { SetValue(IsSafeEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsSafeEnabledProperty = DependencyProperty.Register(nameof(IsSafeEnabled), typeof(bool), 
            typeof(PasswordBox), new PropertyMetadata(true, OnIsSafeEnabledChanged));

        private static void OnIsSafeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = (PasswordBox)d;
            p.SetCurrentValue(UnsafePasswordProperty, !(bool)e.NewValue ? p.Password : string.Empty);
        }

        public string UnsafePassword
        {
            get { return (string)GetValue(UnsafePasswordProperty); }
            set { SetValue(UnsafePasswordProperty, value); }
        }
        public static readonly DependencyProperty UnsafePasswordProperty = DependencyProperty.Register(nameof(UnsafePassword), typeof(string), 
            typeof(PasswordBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUnsafePasswordChanged));

        private static void OnUnsafePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = (PasswordBox)d;
            if (!p.IsSafeEnabled)
            {
                p.Password = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether whether to display the  password reveal button.
        /// </summary>
        public bool RevealButtonEnabled
        {
            get { return (bool)GetValue(RevealButtonEnabledProperty); }
            set { SetValue(RevealButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty RevealButtonEnabledProperty = DependencyProperty.Register(nameof(RevealButtonEnabled), typeof(bool),
            typeof(PasswordBox), new PropertyMetadata(true));

        /// <summary>
        /// Event fired from this text box when its inner content
        /// has been changed.
        /// </summary>
        /// <remarks>
        /// It is redirected from inner TextContainer.Changed event.
        /// </remarks>
        public event RoutedEventHandler PasswordChanged
        {
            add => AddHandler(PasswordChangedEvent, value);
            remove => RemoveHandler(PasswordChangedEvent, value);
        }
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(nameof(PasswordChanged), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PasswordBox));

        public PasswordBox()
        {
            _lockUpdatingContents = false;
        }

        /// <inheritdoc />
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            UpdateTextContents(true);

            if (_lockUpdatingContents) { base.OnTextChanged(e); }
            else
            {
                if (PlaceholderEnabled && Text.Length > 0) { SetCurrentValue(PlaceholderEnabledProperty, false); }

                if (!PlaceholderEnabled && Text.Length < 1) { SetCurrentValue(PlaceholderEnabledProperty, true); }

                RevealClearButton();
            }
        }

        /// <summary>
        /// Triggered by clicking a button in the control template.
        /// </summary>
        /// <param name="parameter">Additional parameters.</param>
        protected override void OnTemplateButtonClick(string? parameter)
        {
            System.Diagnostics.Debug.WriteLine($"INFO: {typeof(PasswordBox)} button clicked with param: {parameter}", "Core.Libraries.WPF.PasswordBox");

            switch (parameter)
            {
                case "reveal":
                    SetCurrentValue(IsPasswordRevealedProperty, !IsPasswordRevealed);
                    _ = Focus();
                    CaretIndex = Text.Length;
                    break;
                default:
                    base.OnTemplateButtonClick(parameter);
                    break;
            }
        }

        private void UpdateTextContents(bool isTriggeredByTextInput)
        {
            if (_lockUpdatingContents) { return; }

            if (IsPasswordRevealed)
            {
                if (Password == Text) { return; }

                _lockUpdatingContents = true;

                if (isTriggeredByTextInput) { SetCurrentValue(PasswordProperty, Text); }
                else
                {
                    SetCurrentValue(TextProperty, Password);
                    CaretIndex = Text.Length;
                }

                RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

                _lockUpdatingContents = false;

                return;
            }

            var caretIndex = CaretIndex;
            var selectionIndex = SelectionStart;
            var currentPassword = Password;
            var newPasswordValue = currentPassword;

            if (isTriggeredByTextInput)
            {
                var currentText = Text;
                var newCharacters = currentText.Replace(PasswordChar.ToString(), string.Empty);

                if (currentText.Length < currentPassword.Length)
                {
                    newPasswordValue = currentPassword.Remove(selectionIndex, currentPassword.Length - currentText.Length);
                }

                if (newCharacters.Length > 1)
                {
                    var index = currentText.IndexOf(newCharacters[0]);

                    newPasswordValue = index > newPasswordValue.Length - 1 ? newPasswordValue + newCharacters : newPasswordValue.Insert(index, newCharacters);
                }
                else
                {
                    for (int i = 0; i < currentText.Length; i++)
                    {
                        if (currentText[i] == PasswordChar) { continue; }

                        newPasswordValue = currentText.Length == newPasswordValue.Length ? newPasswordValue.Remove(i, 1).Insert(i, currentText[i].ToString()) : newPasswordValue.Insert(i, currentText[i].ToString());
                    }
                }
            }

            _lockUpdatingContents = true;

            SetCurrentValue(TextProperty, new string(PasswordChar, newPasswordValue.Length));
            SetCurrentValue(PasswordProperty, newPasswordValue);
            CaretIndex = caretIndex;

            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

            _lockUpdatingContents = false;
        }

    }
}
