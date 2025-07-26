using Core.Libraries.Helpers.PythonNet;
using Core.Libraries.PythonNet.Attributes;
using System.Linq.Expressions;

namespace Core.Libraries.PythonNet.Utils
{
    [Ops]
    internal static class FlagEnumOps<T> where T : Enum
    {
        static readonly Func<T, T, T> and = BinaryOp(Expression.And);
        static readonly Func<T, T, T> or = BinaryOp(Expression.Or);
        static readonly Func<T, T, T> xor = BinaryOp(Expression.ExclusiveOr);
        static readonly Func<T, T> invert = UnaryOp(Expression.OnesComplement);

#pragma warning disable IDE1006
        public static T op_BitwiseAnd(T a, T b) => and(a, b);
        public static T op_BitwiseOr(T a, T b) => or(a, b);
        public static T op_ExclusiveOr(T a, T b) => xor(a, b);
        public static T op_OnesComplement(T value) => invert(value);
#pragma warning restore IDE1006

        static Expression FromNumber(Expression number) => Expression.Convert(number, typeof(T));

        static Func<T, T, T> BinaryOp(Func<Expression, Expression, BinaryExpression> op)
        {
            return OpsHelper.Binary<T>((a, b) =>
            {
                var numericA = OpsHelper.EnumUnderlyingValue(a);
                var numericB = OpsHelper.EnumUnderlyingValue(b);
                var numericResult = op(numericA, numericB);
                return FromNumber(numericResult);
            });
        }
        static Func<T, T> UnaryOp(Func<Expression, UnaryExpression> op)
        {
            return OpsHelper.Unary<T>(value =>
            {
                var numeric = OpsHelper.EnumUnderlyingValue(value);
                var numericResult = op(numeric);
                return FromNumber(numericResult);
            });
        }
    }
}
