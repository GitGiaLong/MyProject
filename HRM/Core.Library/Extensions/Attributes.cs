using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Core.Library.Extensions
{
    public static class Attributes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetName(this Enum enumValue)
        {
            return enumValue.GetType()?.GetMember(enumValue.ToString())?.First()?
                            .GetCustomAttribute<DisplayAttribute>()?.Name ?? "Not Get Enum Value";
        }

        /// <summary>
        /// true get name of display
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="f"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public static string Display<TProperty>(this Expression<Func<TProperty>> f, bool getType = true)
        {
            var exp = f.Body as MemberExpression;
            var property = exp.Expression.Type.GetProperty(exp.Member.Name);
            var attr = property?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            if (getType == true)
            {
                return attr?.GetName() ?? exp.Member.Name;
            }
            else
            {
                return exp.Member.Name;
            }
        }

        /// <summary>
        /// Get enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToEnumString<T>([NotNull] this T type) where T : Enum
        {
            Type enumType = typeof(T);
            string name = Enum.GetName(enumType, type!)!;
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
            return enumMemberAttribute?.Value ?? "";
        }
    }
}
