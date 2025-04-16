using Libraries.Custom.Enums.Interactivity;
using Libraries.Custom.Interfaces.Interactivity;

namespace Libraries.Custom.Extensions.Interactivity
{
    [ContentProperty(nameof(Conditions))]
    public class ConditionalExpression : Freezable, ICondition
    {
        public ConditionCollection Conditions
        {
            get { return (ConditionCollection)GetValue(ConditionsProperty); }
        }
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(nameof(Conditions), typeof(ConditionCollection), typeof(ConditionalExpression), new PropertyMetadata(null));

        public ForwardChaining ForwardChaining
        {
            get { return (ForwardChaining)GetValue(ForwardChainingProperty); }
            set { SetValue(ForwardChainingProperty, value); }
        }
        public static readonly DependencyProperty ForwardChainingProperty = DependencyProperty.Register(nameof(ForwardChaining), typeof(ForwardChaining), typeof(ConditionalExpression), new PropertyMetadata(ForwardChaining.And));

        protected override Freezable CreateInstanceCore()
        {
            return new ConditionalExpression();
        }

        public ConditionalExpression()
        {
            this.SetValue(ConditionalExpression.ConditionsProperty, new ConditionCollection());
        }

        public bool Evaluate()
        {
            bool result = false;
            foreach (ComparisonCondition operation in this.Conditions)
            {
                result = operation.Evaluate();

                if (result == false && this.ForwardChaining == ForwardChaining.And)
                {
                    return result;
                }

                if (result == true && this.ForwardChaining == ForwardChaining.Or)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
