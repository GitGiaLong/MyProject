using Libraries.Custom.Controls.Inputs.InputControl;
using System.Windows.Controls;

namespace Libraries.Custom.Controls
{
    public class InputControl : Control
    {
        public InputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputControl),
            new FrameworkPropertyMetadata(typeof(InputControl)));
        }
        // Dependency Property cho InputType
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register(nameof(InputType), typeof(InputType),
                typeof(InputControl), new PropertyMetadata(InputType.Text));

        public InputType InputType
        {
            get { return (InputType)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        // Dependency Property cho Value (giá trị đầu vào)
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object),
                typeof(InputControl), new PropertyMetadata(null));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

    }
}
