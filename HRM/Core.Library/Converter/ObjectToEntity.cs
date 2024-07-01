using Newtonsoft.Json;

namespace Core.Library.Converter
{
    public static class ObjectToEntity
    {
        public static T ConvertClassToObject<T>(object? Data = null)
        {
            return JsonConvert.DeserializeObject<T>(Data.ToString());
        }
    }
}
