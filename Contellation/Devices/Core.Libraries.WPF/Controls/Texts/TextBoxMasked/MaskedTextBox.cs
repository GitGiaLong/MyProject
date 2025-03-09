using Core.Libraries.WPF.Controls.Boxs.TextBoxMasked.Filter;
using System.ComponentModel;
using System.Windows.Input;

namespace Core.Libraries.WPF.Controls
{
    public class MaskedTextBox : TextBox
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cashed mask to apply to the textbox
        /// </summary>
        private MaskedTextProvider MaskProviderCashed
        {
            get { return (MaskedTextProvider)GetValue(MaskProviderCashedProperty); }
            set { SetValue(MaskProviderCashedProperty, value); }
        }
        private static readonly DependencyProperty MaskProviderCashedProperty = DependencyProperty.Register(nameof(MaskProviderCashed), typeof(MaskedTextProvider), 
            typeof(MaskedTextBox), new UIPropertyMetadata(null, MaskChanged));

        /// <summary>
        /// Gets or sets the cashed mask format string to apply to the textbox
        /// </summary>
        private string MaskProviderCashedMask
        {
            get { return (string)GetValue(MaskProviderCashedMaskProperty); }
            set { SetValue(MaskProviderCashedMaskProperty, value); }
        }
        private static readonly DependencyProperty MaskProviderCashedMaskProperty = DependencyProperty.Register(nameof(MaskProviderCashedMask), typeof(string), 
            typeof(MaskedTextBox), new UIPropertyMetadata(string.Empty, MaskChanged));

        /// <summary>
        /// Gets or sets the cashed promt char to apply to the textbox mask
        /// </summary>
        private char PromptCharCached
        {
            get { return (char)GetValue(PromptCharCachedProperty); }
            set { SetValue(PromptCharCachedProperty, value); }
        }
        private static readonly DependencyProperty PromptCharCachedProperty = DependencyProperty.Register(nameof(PromptCharCached), typeof(char),
            typeof(MaskedTextBox), new UIPropertyMetadata(' ', MaskChanged));

        /// <summary>
        /// Gets or sets the promt char to apply to the textbox mask
        /// </summary>
        public char PromptChar
        {
            get { return (char)GetValue(PromptCharProperty); }
            set { SetValue(PromptCharProperty, value); }
        }
        public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register(nameof(PromptChar), typeof(char),
            typeof(MaskedTextBox), new UIPropertyMetadata(' ', MaskChanged));

        /// <summary>
        /// Gets or sets the mask to apply to the textbox
        /// </summary>
        public string Mask
        {
            get { return (string)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask), typeof(string),
            typeof(MaskedTextBox), new UIPropertyMetadata(String.Empty, MaskChanged));

        /// <summary>
        /// Gets a predefined filter for the specified RegExp
        /// </summary>
        public TextBoxMaskedFilterType Filter
        {

            get { return (TextBoxMaskedFilterType)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(TextBoxMaskedFilterType),
            typeof(MaskedTextBox), new UIPropertyMetadata(TextBoxMaskedFilterType.Any, MaskChanged));

        //callback for when the Mask property is changed
        static void MaskChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //make sure to update the text if the mask changes
            var textBox = (MaskedTextBox)sender;
            textBox.RefreshText(textBox.MaskProvider, 0);
        }

        /// <summary>
        /// Gets the MaskTextProvider for the specified Mask
        /// </summary>
        public MaskedTextProvider MaskProvider
        {
            get
            {
                if (!IsMaskProviderUpdated()) { return MaskProviderCashed; }
                MaskProviderCashedMask = Mask;
                if (!String.IsNullOrEmpty(MaskProviderCashedMask))
                {
                    MaskProviderCashed = new MaskedTextProvider(MaskProviderCashedMask) { PromptChar = this.PromptChar };
                }
                else
                {
                    MaskProviderCashed = null;
                }

                if (MaskProviderCashed != null) { MaskProviderCashed.Set(Text); }
                return MaskProviderCashed;
            }
        }

        /// <summary>
        /// Check, is configuration of MaskProvider changed
        /// </summary>
        /// <returns><c>true</c>, if it was changed and <c>false</c>, if it wasn't.</returns>
        private bool IsMaskProviderUpdated()
        {
            bool result = false;
            if (MaskProviderCashedMask != Mask)
            {
                MaskProviderCashedMask = Mask;
                result = true;
            }
            if (PromptCharCached != PromptChar)
            {
                PromptCharCached = PromptChar;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Gets the RegExFilter for the validation Mask.
        /// </summary>
        private BaseFilter FilterValidator
        {
            get { return TextBoxMaskedFilterProvider.Instance.FilterForMaskedType(Filter); ; }
        }

        #endregion

        /// <summary>
        /// Static Constructor
        /// </summary>
        static MaskedTextBox()
        {
            //override the meta data for the Text Proeprty of the textbox 
            var metaData = new FrameworkPropertyMetadata { CoerceValueCallback = ForceText };
            TextProperty.OverrideMetadata(typeof(MaskedTextBox), metaData);
        }

        //force the text of the control to use the mask
        private static object ForceText(DependencyObject sender, object value)
        {
            var textBox = (MaskedTextBox)sender;
            if (!string.IsNullOrEmpty(textBox.Mask))
            {
                var provider = textBox.MaskProvider;
                if (provider != null)
                {
                    provider.Set((string)value);
                    return provider.ToDisplayString();
                }
            }
            return value;
        }

        ///<summary>
        /// Default  constructor
        ///</summary>
        public MaskedTextBox()
        {
            //cancel the paste and cut command
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, CancelCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CancelCommand));
        }

        //cancel the command
        private static void CancelCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        #region Overrides

        /// <summary>
        /// override this method to replace the characters enetered with the mask
        /// </summary>
        /// <param name="e">Arguments for event</param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            //if the text is readonly do not add the text
            if (IsReadOnly)
            {
                e.Handled = true;
                return;
            }

            int position = SelectionStart;
            var provider = MaskProvider;
            bool ifIsPositionInMiddle = position < Text.Length;
            if (provider != null)
            {
                if (ifIsPositionInMiddle)
                {
                    position = GetNextCharacterPosition(position);

                    if (Keyboard.IsKeyToggled(Key.Insert))
                    {
                        if (provider.Replace(e.Text, position)) { position++; }
                    }
                    else
                    {
                        if (provider.InsertAt(e.Text, position)) { position++; }
                    }

                    position = GetNextCharacterPosition(position);
                }

                RefreshText(provider, position);
                e.Handled = true;
            }

            string textToText = (ifIsPositionInMiddle) ? Text.Insert(position, e.Text) : String.Format("{0}{1}", Text, e.Text);
            if (!FilterValidator.IsTextValid(textToText)) { e.Handled = true; }
            base.OnPreviewTextInput(e);
        }

        /// <summary>
        /// override the key down to handle delete of a character
        /// </summary>
        /// <param name="e">Arguments for the event</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var provider = MaskProvider;
            if (provider == null) { return; }

            int position = SelectionStart;
            switch (e.Key)
            {
                case Key.Delete:
                    if (position < Text.Length)
                    {
                        if (provider.RemoveAt(position)) { RefreshText(provider, position); }
                        e.Handled = true;
                    }
                    break;
                case Key.Space:
                    if (provider.InsertAt(" ", position)) { RefreshText(provider, position); }
                    e.Handled = true;
                    break;
                case Key.Back:
                    if (position > 0)
                    {
                        position--;
                        if (provider.RemoveAt(position)) { RefreshText(provider, position); }
                    }
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// refreshes the text of the textbox
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="position"></param>
        private void RefreshText(MaskedTextProvider provider, int position)
        {
            if (provider != null)
            {
                Text = provider.ToDisplayString();
                SelectionStart = position;
            }
        }

        /// <summary>
        /// gets the next position in the textbox to move
        /// </summary>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        private int GetNextCharacterPosition(int startPosition)
        {
            if (MaskProvider != null)
            {
                int position = MaskProvider.FindEditPositionFrom(startPosition, true);
                if (position != -1) { return position; }
            }
            return startPosition;
        }
        #endregion
    }
}
