using System.Reflection;

namespace Core.Libraries.PythonNet
{
    static internal class ParameterInfoExtensions
    {
        public static object? GetDefaultValue(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.HasDefaultValue) { return parameterInfo.DefaultValue; }
            else
            {
                /* 
                 * [OptionalAttribute] was specified for the parameter. 
                 * See https://stackoverflow.com/questions/3416216/optionalattribute-parameters-default-value 
                 * for rules on determining the value to pass to the parameter 
                 */
                var type = parameterInfo.ParameterType;
                if (type == typeof(object)) { return Type.Missing; }
                else if (type.IsValueType) { return Activator.CreateInstance(type); }
                else { return null; }
            }
        }
    }
}
