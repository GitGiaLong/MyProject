using Core.WPF.Interactivities.Enums;
using Core.WPF.Interactivities.Interfaces;
using System.Windows;
using System.Windows.Markup;

namespace Core.WPF.Interactivities.Extensions
{
    [ContentProperty("Conditions")]
    public class ConditionalExpression : Freezable, ICondition
    {
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register("Conditions", typeof(ConditionCollection), typeof(ConditionalExpression), new PropertyMetadata(null));
        public static readonly DependencyProperty ForwardChainingProperty = DependencyProperty.Register("ForwardChaining", typeof(ForwardChaining), typeof(ConditionalExpression), new PropertyMetadata(ForwardChaining.And));
        
        protected override Freezable CreateInstanceCore()
        {
            return new ConditionalExpression();
        }

        public ForwardChaining ForwardChaining
        {
            get { return (ForwardChaining)GetValue(ForwardChainingProperty); }
            set { SetValue(ForwardChainingProperty, value); }
        }
        public ConditionCollection Conditions
        {
            get { return (ConditionCollection)GetValue(ConditionsProperty); }
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
