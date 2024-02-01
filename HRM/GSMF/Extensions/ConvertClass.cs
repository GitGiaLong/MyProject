using Newtonsoft.Json;

namespace GSMF.Extensions
{
    public class ConvertClass
    {
        public T ConvertClassToObject<T>(object? Data = null)
        {
            return JsonConvert.DeserializeObject<T>(Data.ToString());
        }
    }
}
