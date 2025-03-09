using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Media.Animation;

namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class ChangePropertyAction : TargetedTriggerAction<object>
    {
        public string PropertyName
        {
            get { return (string)this.GetValue(ChangePropertyAction.PropertyNameProperty); }
            set { this.SetValue(ChangePropertyAction.PropertyNameProperty, value); }
        }
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string), 
            typeof(ChangePropertyAction), null);

        public object Value
        {
            get { return this.GetValue(ChangePropertyAction.ValueProperty); }
            set { this.SetValue(ChangePropertyAction.ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), 
            typeof(ChangePropertyAction), null);

        public Duration Duration
        {
            get { return (Duration)this.GetValue(ChangePropertyAction.DurationProperty); }
            set { this.SetValue(ChangePropertyAction.DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(Duration), 
            typeof(ChangePropertyAction), null);

        public bool Increment
        {
            get { return (bool)this.GetValue(ChangePropertyAction.IncrementProperty); }
            set { this.SetValue(ChangePropertyAction.IncrementProperty, value); }
        }
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(bool), typeof(ChangePropertyAction), null);

        public ChangePropertyAction() { }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null) { return; }
            if (string.IsNullOrEmpty(this.PropertyName)) { return; }
            if (this.Target == null) { return; }

            Type targetType = this.Target.GetType();
            PropertyInfo propertyInfo = targetType.GetProperty(this.PropertyName);
            this.ValidateProperty(propertyInfo);

            object newValue = this.Value;
            TypeConverter converter = TypeConverterHelper.GetTypeConverter(propertyInfo.PropertyType);

            Exception innerException = null;
            try
            {
                if (this.Value != null)
                {
                    if (converter != null && converter.CanConvertFrom(this.Value.GetType()))
                    {
                        newValue = converter.ConvertFrom(context: null, culture: CultureInfo.InvariantCulture, value: this.Value);
                    }
                    else
                    {
                        converter = TypeConverterHelper.GetTypeConverter(this.Value.GetType());
                        if (converter != null && converter.CanConvertTo(propertyInfo.PropertyType))
                        {
                            newValue = converter.ConvertTo(context: null, culture: CultureInfo.InvariantCulture,
                                value: this.Value, destinationType: propertyInfo.PropertyType);
                        }
                    }
                }

                if (this.Duration.HasTimeSpan)
                {
                    this.ValidateAnimationPossible(targetType);
                    object fromValue = ChangePropertyAction.GetCurrentPropertyValue(this.Target, propertyInfo);
                    this.AnimatePropertyChange(propertyInfo, fromValue, newValue);
                }
                else
                {
                    if (this.Increment)
                    {
                        newValue = this.IncrementCurrentValue(propertyInfo);
                    }
                    propertyInfo.SetValue(this.Target, newValue, new object[0]);
                }
            }
            catch (FormatException e)
            {
                innerException = e;
            }
            catch (ArgumentException e)
            {
                innerException = e;
            }
            catch (MethodAccessException e)
            {
                innerException = e;
            }
            if (innerException != null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.ChangePropertyActionCannotSetValueExceptionMessage,
                    this.Value != null ? this.Value.GetType().Name : "null", this.PropertyName, propertyInfo.PropertyType.Name), innerException);
            }
        }

        private void AnimatePropertyChange(PropertyInfo propertyInfo, object fromValue, object newValue)
        {
            Storyboard sb = new Storyboard();
            Timeline timeline;
            if (typeof(double).IsAssignableFrom(propertyInfo.PropertyType))
            {
                timeline = this.CreateDoubleAnimation((double)fromValue, (double)newValue);
            }
            else if (typeof(Color).IsAssignableFrom(propertyInfo.PropertyType))
            {
                timeline = this.CreateColorAnimation((Color)fromValue, (Color)newValue);
            }
            else if (typeof(Point).IsAssignableFrom(propertyInfo.PropertyType))
            {
                timeline = this.CreatePointAnimation((Point)fromValue, (Point)newValue);
            }
            else
            {
                timeline = this.CreateKeyFrameAnimation(fromValue, newValue);
            }

            timeline.Duration = this.Duration;
            sb.Children.Add(timeline);

            if (this.TargetObject == null && this.TargetName != null && this.Target is Freezable)
            {
                Storyboard.SetTargetName(sb, this.TargetName);
            }
            else
            {
                Storyboard.SetTarget(sb, (DependencyObject)this.Target);
            }
            Storyboard.SetTargetProperty(sb, new PropertyPath(propertyInfo.Name));

            sb.Completed += (o, e) =>
            {
                propertyInfo.SetValue(this.Target, newValue, new object[0]);
            };
            sb.FillBehavior = FillBehavior.Stop;

            FrameworkElement containingObject = this.AssociatedObject as FrameworkElement;
            if (containingObject != null)
            {
                sb.Begin(containingObject);
            }
            else
            {
                sb.Begin();
            }
        }

        private static object GetCurrentPropertyValue(object target, PropertyInfo propertyInfo)
        {
            FrameworkElement targetElement = target as FrameworkElement;
            Type targetType = target.GetType();
            object fromValue = propertyInfo.GetValue(target, null);

            if (targetElement != null && (propertyInfo.Name == "Width" || propertyInfo.Name == "Height") && Double.IsNaN((double)fromValue))
            {
                if (propertyInfo.Name == "Width")
                {
                    fromValue = targetElement.ActualWidth;
                }
                else
                {
                    fromValue = targetElement.ActualHeight;
                }
            }

            return fromValue;
        }

        private void ValidateAnimationPossible(Type targetType)
        {
            if (this.Increment)
            {
                throw new InvalidOperationException(ExceptionStringTable.ChangePropertyActionCannotIncrementAnimatedPropertyChangeExceptionMessage);
            }
            if (!typeof(DependencyObject).IsAssignableFrom(targetType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.ChangePropertyActionCannotAnimateTargetTypeExceptionMessage, targetType.Name));
            }
        }

        private Timeline CreateKeyFrameAnimation(object newValue, object fromValue)
        {
            ObjectAnimationUsingKeyFrames objectAnimation = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame k1 = new DiscreteObjectKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0)),
                Value = fromValue,
            };
            DiscreteObjectKeyFrame k2 = new DiscreteObjectKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(this.Duration.TimeSpan),
                Value = newValue,
            };

            objectAnimation.KeyFrames.Add(k1);
            objectAnimation.KeyFrames.Add(k2);

            return objectAnimation;
        }

        private Timeline CreatePointAnimation(Point fromValue, Point newValue)
        {
            return new PointAnimation()
            {
                From = (Point)fromValue,
                To = (Point)newValue,
            };
        }

        private Timeline CreateColorAnimation(Color fromValue, Color newValue)
        {
            return new ColorAnimation()
            {
                From = fromValue,
                To = newValue,
            };
        }

        private Timeline CreateDoubleAnimation(double fromValue, double newValue)
        {
            return new DoubleAnimation()
            {
                From = fromValue,
                To = newValue,
            };
        }

        private void ValidateProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ExceptionStringTable.ChangePropertyActionCannotFindPropertyNameExceptionMessage,
                    this.PropertyName,
                    this.Target.GetType().Name));
            }

            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ExceptionStringTable.ChangePropertyActionPropertyIsReadOnlyExceptionMessage,
                    this.PropertyName,
                    this.Target.GetType().Name));
            }
        }

        private object IncrementCurrentValue(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    ExceptionStringTable.ChangePropertyActionCannotIncrementWriteOnlyPropertyExceptionMessage,
                    propertyInfo.Name));
            }

            object currentValue = propertyInfo.GetValue(this.Target, null);
            object returnValue = currentValue;

            Type propertyType = propertyInfo.PropertyType;
            TypeConverter converter = TypeConverterHelper.GetTypeConverter(propertyInfo.PropertyType);
            object value = this.Value;

            if (value == null || currentValue == null)
            {
                return value;
            }

            if (converter.CanConvertFrom(value.GetType()))
            {
                value = TypeConverterHelper.DoConversionFrom(converter, value);
            }

            if (typeof(double).IsAssignableFrom(propertyType))
            {
                returnValue = (double)currentValue + (double)value;
            }
            else if (typeof(int).IsAssignableFrom(propertyType))
            {
                returnValue = (int)currentValue + (int)value;
            }
            else if (typeof(float).IsAssignableFrom(propertyType))
            {
                returnValue = (float)currentValue + (float)value;
            }
            else if (typeof(string).IsAssignableFrom(propertyType))
            {
                returnValue = (string)currentValue + (string)value;
            }
            else
            {
                returnValue = TryAddition(currentValue, value);
            }
            return returnValue;
        }

        private static object TryAddition(object currentValue, object value)
        {
            object returnValue = null;
            Type valueType = value.GetType();
            Type additiveType = currentValue.GetType();

            MethodInfo uniqueAdditionOperation = null;
            object convertedValue = value;

            foreach (MethodInfo additionOperation in additiveType.GetMethods())
            {
                if (string.Compare(additionOperation.Name, "op_Addition", StringComparison.Ordinal) != 0)
                {
                    continue;
                }

                ParameterInfo[] parameters = additionOperation.GetParameters();

                Debug.Assert(parameters.Length == 2, "op_Addition is expected to have 2 parameters");

                Type secondParameterType = parameters[1].ParameterType;
                if (!parameters[0].ParameterType.IsAssignableFrom(additiveType))
                {
                    continue;
                }
                else if (!secondParameterType.IsAssignableFrom(valueType))
                {
                    TypeConverter additionConverter = TypeConverterHelper.GetTypeConverter(secondParameterType);
                    if (additionConverter.CanConvertFrom(valueType))
                    {
                        convertedValue = TypeConverterHelper.DoConversionFrom(additionConverter, value);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (uniqueAdditionOperation != null)
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        ExceptionStringTable.ChangePropertyActionAmbiguousAdditionOperationExceptionMessage,
                        additiveType.Name));
                }
                uniqueAdditionOperation = additionOperation;
            }

            if (uniqueAdditionOperation != null)
            {
                returnValue = uniqueAdditionOperation.Invoke(null, new object[] { currentValue, convertedValue });
            }
            else
            {
                returnValue = value;
            }

            return returnValue;
        }
    }
}
