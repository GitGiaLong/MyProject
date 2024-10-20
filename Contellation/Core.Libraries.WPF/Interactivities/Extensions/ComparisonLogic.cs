﻿using Core.Libraries.WPF.Interactivities.Enums;
using System.ComponentModel;
using System.Globalization;

namespace Core.Libraries.WPF.Interactivities
{
    internal static class ComparisonLogic
    {
        internal static bool EvaluateImpl(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
        {
            bool result = false;

            if (leftOperand != null)
            {
                Type leftType = leftOperand.GetType();

                if (rightOperand != null)
                {
                    TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(leftType);
                    rightOperand = TypeConverterHelper.DoConversionFrom(typeConverter, rightOperand);
                }
            }

            IComparable leftComparableOperand = leftOperand as IComparable;
            IComparable rightComparableOperand = rightOperand as IComparable;

            if (leftComparableOperand != null && rightComparableOperand != null)
            {
                return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
            }

            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    result = Equals(leftOperand, rightOperand);
                    break;
                case ComparisonConditionType.NotEqual:
                    result = !Equals(leftOperand, rightOperand);
                    break;

                case ComparisonConditionType.GreaterThan:
                case ComparisonConditionType.GreaterThanOrEqual:
                case ComparisonConditionType.LessThan:
                case ComparisonConditionType.LessThanOrEqual:
                    if (leftComparableOperand == null && rightComparableOperand == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidOperands,
                            leftOperand != null ? leftOperand.GetType().Name : "null", rightOperand != null ? rightOperand.GetType().Name : "null",
                            operatorType.ToString()));
                    }
                    else if (leftComparableOperand == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidLeftOperand,
                            leftOperand != null ? leftOperand.GetType().Name : "null", operatorType.ToString()));
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.InvalidRightOperand,
                            rightOperand != null ? rightOperand.GetType().Name : "null", operatorType.ToString()));
                    }
            }
            return result;
        }

        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            object convertedOperand = null;

            try
            {
                convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
            }
            catch (FormatException) { }
            catch (InvalidCastException) { }

            if (convertedOperand == null)
            {
                return operatorType == ComparisonConditionType.NotEqual;
            }

            int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
            bool result = false;

            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    result = comparison == 0;
                    break;
                case ComparisonConditionType.GreaterThan:
                    result = comparison > 0;
                    break;
                case ComparisonConditionType.GreaterThanOrEqual:
                    result = comparison >= 0;
                    break;
                case ComparisonConditionType.LessThan:
                    result = comparison < 0;
                    break;
                case ComparisonConditionType.LessThanOrEqual:
                    result = comparison <= 0;
                    break;
                case ComparisonConditionType.NotEqual:
                    result = comparison != 0;
                    break;
            }
            return result;
        }
    }
}
