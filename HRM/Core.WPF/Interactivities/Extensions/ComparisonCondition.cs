using Core.WPF.Interactivities.Enums;

namespace Core.WPF.Interactivities.Extensions
{
    public class ComparisonCondition : Freezable
    {
        public static readonly DependencyProperty LeftOperandProperty = DependencyProperty.Register("LeftOperand", typeof(object), typeof(ComparisonCondition), new PropertyMetadata(null));
        public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register("Operator", typeof(ComparisonConditionType), typeof(ComparisonCondition), new PropertyMetadata(ComparisonConditionType.Equal));
        public static readonly DependencyProperty RightOperandProperty = DependencyProperty.Register("RightOperand", typeof(object), typeof(ComparisonCondition), new PropertyMetadata(null));
        protected override Freezable CreateInstanceCore() { return new ComparisonCondition(); }

        public object LeftOperand
        {
            get { return GetValue(LeftOperandProperty); }
            set { SetValue(LeftOperandProperty, value); }
        }

        public object RightOperand
        {
            get { return GetValue(RightOperandProperty); }
            set { SetValue(RightOperandProperty, value); }
        }

        public ComparisonConditionType Operator
        {
            get { return (ComparisonConditionType)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }

        public bool Evaluate()
        {
            this.EnsureBindingUpToDate();
            return ComparisonLogic.EvaluateImpl(this.LeftOperand, this.Operator, this.RightOperand);
        }

        private void EnsureBindingUpToDate()
        {
            DataBindingHelper.EnsureBindingUpToDate(this, LeftOperandProperty);
            DataBindingHelper.EnsureBindingUpToDate(this, OperatorProperty);
            DataBindingHelper.EnsureBindingUpToDate(this, RightOperandProperty);
        }
    }
}
