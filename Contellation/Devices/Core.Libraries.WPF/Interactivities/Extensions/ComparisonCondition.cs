using Core.Libraries.WPF.Interactivities.Enums;

namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class ComparisonCondition : Freezable
    {
        public object LeftOperand
        {
            get { return GetValue(LeftOperandProperty); }
            set { SetValue(LeftOperandProperty, value); }
        }
        public static readonly DependencyProperty LeftOperandProperty = DependencyProperty.Register(nameof(LeftOperand), typeof(object), 
            typeof(ComparisonCondition), new PropertyMetadata(null));

        public ComparisonConditionType Operator
        {
            get { return (ComparisonConditionType)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }
        public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register(nameof(Operator), typeof(ComparisonConditionType), 
            typeof(ComparisonCondition), new PropertyMetadata(ComparisonConditionType.Equal));

        public object RightOperand
        {
            get { return GetValue(RightOperandProperty); }
            set { SetValue(RightOperandProperty, value); }
        }
        public static readonly DependencyProperty RightOperandProperty = DependencyProperty.Register(nameof(RightOperand), typeof(object), 
            typeof(ComparisonCondition), new PropertyMetadata(null));

        protected override Freezable CreateInstanceCore() { return new ComparisonCondition(); }

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
