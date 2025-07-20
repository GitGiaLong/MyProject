using Core.Libraries.PythonNet.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Libraries.Helpers.PythonNet
{
    static class OpsHelper
    {
        public static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.Static;

        public static Func<T, T, T> Binary<T>(Func<Expression, Expression, Expression> func)
        {
            var a = Expression.Parameter(typeof(T), "a");
            var b = Expression.Parameter(typeof(T), "b");
            var body = func(a, b);
            var lambda = Expression.Lambda<Func<T, T, T>>(body, a, b);
            return lambda.Compile();
        }

        public static Func<T, T> Unary<T>(Func<Expression, Expression> func)
        {
            var value = Expression.Parameter(typeof(T), "value");
            var body = func(value);
            var lambda = Expression.Lambda<Func<T, T>>(body, value);
            return lambda.Compile();
        }

        public static bool IsOpsHelper(this MethodBase method)
            => method.DeclaringType.GetCustomAttribute<OpsAttribute>() is not null;

        public static Expression EnumUnderlyingValue(Expression enumValue)
            => Expression.Convert(enumValue, enumValue.Type.GetEnumUnderlyingType());
    }
}
