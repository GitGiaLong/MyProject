namespace Core.Libraries.Models
{
    public class ApiRestFul<T> : Response, IApiRestFul<T>
    {
        private static T? _Data = default(T?);
        public T? Data { get { return _Data; } set { _Data = value; } }
    }
}
