namespace Entities.Application.Convert
{
    public class ApiRestFul<T> : ApiResponse 
    {
        public T? Data { get; set; } = default(T?);
    }

}
