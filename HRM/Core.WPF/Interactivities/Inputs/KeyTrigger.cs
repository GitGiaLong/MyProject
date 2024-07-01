using Core.WPF.Interactivities.Enums;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Core.WPF.Interactivities.Inputs
{
    public class KeyTrigger : EventTriggerBase<UIElement>
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyTrigger));

        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyTrigger));

        public static readonly DependencyProperty ActiveOnFocusProperty = DependencyProperty.Register("ActiveOnFocus", typeof(bool), typeof(KeyTrigger));

        public static readonly DependencyProperty FiredOnProperty = DependencyProperty.Register("FiredOn", typeof(KeyTriggerFiredOn), typeof(KeyTrigger));

        private UIElement targetElement;

        public Key Key
        {
            get { return (Key)this.GetValue(KeyTrigger.KeyProperty); }
            set { this.SetValue(KeyTrigger.KeyProperty, value); }
        }

        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)this.GetValue(KeyTrigger.ModifiersProperty); }
            set { this.SetValue(KeyTrigger.ModifiersProperty, value); }
        }

        public bool ActiveOnFocus
        {
            get { return (bool)this.GetValue(KeyTrigger.ActiveOnFocusProperty); }
            set { this.SetValue(KeyTrigger.ActiveOnFocusProperty, value); }
        }

        public KeyTriggerFiredOn FiredOn
        {
            get { return (KeyTriggerFiredOn)this.GetValue(KeyTrigger.FiredOnProperty); }
            set { this.SetValue(KeyTrigger.FiredOnProperty, value); }
        }

        protected override string GetEventName()
        {
            return "Loaded";
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            bool isKeyMatch = e.Key == this.Key;

            ModifierKeys actualModifiers = GetActualModifiers();

            bool isModifiersMatch = actualModifiers == this.Modifiers;

            if (isKeyMatch && isModifiersMatch)
            {
                this.InvokeActions(e);
            }
        }

        private static ModifierKeys GetActualModifiers()
        {
            ModifierKeys actualModifiers = ModifierKeys.None;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                actualModifiers |= ModifierKeys.Control;
            }
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                actualModifiers |= ModifierKeys.Shift;
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt) || Keyboard.IsKeyDown(Key.System))
            {
                actualModifiers |= ModifierKeys.Alt;
            }

            return actualModifiers;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (this.ActiveOnFocus)
            {
                this.targetElement = this.Source;
            }
            else
            {
                this.targetElement = KeyTrigger.GetRoot(this.Source);
            }

            if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
            {
                this.targetElement.KeyDown += this.OnKeyPress;
            }
            else
            {
                this.targetElement.KeyUp += this.OnKeyPress;
            }

            UnregisterLoaded(Source as FrameworkElement);
        }

        protected override void OnDetaching()
        {
            if (this.targetElement != null)
            {
                if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
                {
                    this.targetElement.KeyDown -= this.OnKeyPress;
                }
                else
                {
                    this.targetElement.KeyUp -= this.OnKeyPress;
                }
            }

            base.OnDetaching();
        }

        private static UIElement GetRoot(DependencyObject current)
        {
            UIElement last = null;

            while (current != null)
            {
                last = current as UIElement;
                current = VisualTreeHelper.GetParent(current);
            }

            return last;
        }
    }
}
