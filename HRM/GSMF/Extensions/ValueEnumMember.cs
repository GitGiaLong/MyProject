using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace GSMF.Extensions
{
    public static class ValueEnumMember
    {
        public static string? ToEnumString<T>([NotNull] this T type) where T : Enum
        {
            Type enumType = typeof(T);
            string name = Enum.GetName(enumType, type!)!;
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
            return enumMemberAttribute?.Value;
        }
    }
}
